namespace TicTacToe.Domain.Entities.BaseModule
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        protected BaseEntity(Guid id)
        {
            Id = id;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not BaseEntity other) return false;
            return Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
