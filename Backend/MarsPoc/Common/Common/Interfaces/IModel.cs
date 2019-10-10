namespace Common.Interfaces
{
    public interface IModel
    {
        int Id { get; set; }

        void Copy(IModel item);
    }
}
