using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabularyService.Domain;

public sealed class Vocabulary : AggregateRoot, IAuditable, ISoftDeletable
{
    public string Word { get; private set; } = string.Empty;
    public string PartOfSpeech { get; private set; } = string.Empty;
    public string? Pronunciation { get; private set; }
    public string? Cloze { get; private set; }
    public string? Definition { get; private set; }
    public string? Example { get; private set; }
    public string? Meaning { get; private set; }

    private readonly AuditState _audit = new();
    private readonly SoftDeleteState _deletion = new();

    // IAuditable
    public DateTime CreatedAt => _audit.CreatedAt;
    public string? CreatedBy => _audit.CreatedBy;
    public DateTime? LastModifiedAt => _audit.LastModifiedAt;
    public string? LastModifiedBy => _audit.LastModifiedBy;

    // ISoftDeletable
    public bool IsDeleted => _deletion.IsDeleted;
    public DateTime? DeletedAt => _deletion.DeletedAt;
    public string? DeletedBy => _deletion.DeletedBy;

    public void SoftDelete(string? deletedBy)
        => _deletion.MarkDeleted(deletedBy);

    public void Restore()
        => _deletion.MarkRestored();
}
