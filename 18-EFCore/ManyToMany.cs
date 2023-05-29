using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_EFCore
{
    internal class ManyToMany
    {
    }

    public class Post1
    {
        public int Id { get; set; }
        public List<PostTag> PostTags { get; } = new();
    }

    public class Tag
    {
        public int Id { get; set; }
        public List<PostTag> PostTags { get; } = new();
    }

    public class PostTag
    {
        public int PostsId { get; set; }
        public int TagsId { get; set; }
        public Post1 Post { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
