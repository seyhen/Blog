namespace Blog.Models
{
    public interface IService<T>
    {
        public IEnumerable<T> Get();

        public T Get(string id);

        public void Create(T blog);
    
    }
}
