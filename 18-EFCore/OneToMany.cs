using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_EFCore
{
    internal class OneToMany
    {
    }

    // Principal (parent)
    public class Blog
    {
        public int Id { get; set; }
        public ICollection<Post> Posts { get; } = new List<Post>(); // 包含依赖项的集合导航
    }

    // Dependent (child)
    public class Post
    {
        public int Id { get; set; }
        public int BlogId { get; set; } // 必需的外键属性
        public Blog Blog { get; set; } = null!; // 必需的指向主体的参考导航
    }
}
