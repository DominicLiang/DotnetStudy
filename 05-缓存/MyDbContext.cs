namespace _05_缓存
{
    public class MyDbContext
    {
        public static async Task<Book?> GetByIdAsync(int id)
        {
            return await Task.Run(() => GetById(id));
        }

        public static Book? GetById(int id) => id switch
        {
            1 => new Book(1, "C#"),
            2 => new Book(2, "C++"),
            3 => new Book(3, "Python"),
            _ => null
        };
    }
}
