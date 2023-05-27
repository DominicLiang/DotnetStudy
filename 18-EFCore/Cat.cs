using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_EFCore
{
    [Table("T_Cats")]
    public class Cat
    {
        // 还有很多其他特性 看https://learn.microsoft.com/zh-cn/dotnet/api/system.componentmodel.dataannotations.datatypeattribute?view=net-7.0

        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]// StringLength 指定数据字段中允许的字符的最小长度和最大长度
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]// 使属性成为不可为空
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]// 使FirstMidName属性映射到数据库中的FirstName列
        [Display(Name = "First Name")]// 特性指定文本框的标题应是“名”、“姓”、“全名”和“注册日期”，默认标题没有分隔单词的空格，例如“Lastname”
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)] // DataType 指定比数据库内部更具体的数据类型，如日期、时间、电话号码、货币、电子邮件等
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] // DisplayFormat 用于显式指定日期格式， ApplyFormatInEditMode设置指定对编辑UI应用该格式设置
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

        [NotMapped] // 不映射
        public string test { get; set; }
    }
}
