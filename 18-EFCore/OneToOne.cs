using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_EFCore
{
    internal class OneToOne
    {
    }

    public class Blog1
    {
        public int Id { get; set; }
        public BlogHeader? Header { get; set; } // 从属的引用导航
    }

    // Dependent (child)
    public class BlogHeader
    {
        public int Id { get; set; }
        public int BlogId { get; set; } // 必需的外键属性
        public Blog1 Blog { get; set; } = null!; // 必需的指向主体的参考导航
    }
}
