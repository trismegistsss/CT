namespace CT.Models
{
    public interface IReactiveWrapper<T>
    {
        void Wrap(T model);
        T Unwrap();
    }
}
