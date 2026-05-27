using DevolucionesGarantias.Application.Common.Interfaces;
using DevolucionesGarantias.Application.Requests.DTOs;
using DevolucionesGarantias.Application.Requests.Interfaces;
using DevolucionesGarantias.Application.Requests.Mappings;
using DevolucionesGarantias.Application.Requests.Validators;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Application.Requests.Services;

public sealed class EvidenceService
{
    private const string DefaultEvidenceBucket = "evidence-files";
    private const int MaxEvidencePerRequest = 10;
    private readonly ISolicitudRepository _requests;
    private readonly IEvidenciaRepository _evidence;
    private readonly IFileStorageService _storage;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UploadEvidenceValidator _validator;

    public EvidenceService(
        ISolicitudRepository requests,
        IEvidenciaRepository evidence,
        IFileStorageService storage,
        IUnitOfWork unitOfWork,
        UploadEvidenceValidator validator)
    {
        _requests = requests;
        _evidence = evidence;
        _storage = storage;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<IReadOnlyCollection<EvidenceDto>> ListByRequestAsync(Guid requestId, Guid customerId, CancellationToken cancellationToken = default)
    {
        var request = await _requests.GetByIdAsync(requestId, cancellationToken)
            ?? throw new BusinessRuleException("Solicitud no encontrada.");

        if (request.ClienteId != customerId)
        {
            throw new BusinessRuleException("La solicitud no pertenece al cliente indicado.");
        }

        var evidence = await _evidence.ListByRequestAsync(requestId, cancellationToken);
        return evidence.Select(item => item.ToDto()).ToArray();
    }

    public async Task<EvidenceDto> UploadAsync(UploadEvidenceDto request, Guid uploadedByUserId, CancellationToken cancellationToken = default)
    {
        _validator.ValidateAndThrow(request);

        var solicitud = await _requests.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new BusinessRuleException("Solicitud no encontrada.");

        if (solicitud.ClienteId != uploadedByUserId)
        {
            throw new BusinessRuleException("La solicitud no pertenece al cliente indicado.");
        }

        var currentEvidence = await _evidence.ListByRequestAsync(solicitud.Id, cancellationToken);
        if (currentEvidence.Count >= MaxEvidencePerRequest)
        {
            throw new BusinessRuleException($"La solicitud ya tiene el maximo permitido de {MaxEvidencePerRequest} evidencias.");
        }

        var stored = await _storage.StoreAsync(
            DefaultEvidenceBucket,
            request.FileName,
            request.Content,
            request.ContentType,
            $"evidence/{solicitud.Id:D}",
            cancellationToken);

        var evidence = new Evidencia(
            solicitud.Id,
            request.Type,
            new FilePath(stored.Bucket, stored.Path, stored.Url),
            stored.OriginalFileName,
            stored.SizeInBytes,
            uploadedByUserId.ToString());

        await _evidence.AddAsync(evidence, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return evidence.ToDto();
    }
}
