using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace _07_Lambda和LINQ;

// LINQ to Object   (Enumerable)
// LINQ to SQL      (Queryable)      SQL+ADO.NET
// LINQ to XML

// 1.匿名方法 lambda表达式
// 2.匿名类 var 扩展方法
// 3.linq to object
// 4.yield迭代器
// 5.linq常用方法介绍

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }

}

public class LambdaShow
{
    public delegate void NoReturnNoPara();

    public void Show()
    {
        int k = 1;
        {
            // .net 1.0 不能访问外部变量
            NoReturnNoPara method = DoNothing;
        }
        {
            // .net 2.0 匿名方法
            NoReturnNoPara method = delegate ()
            {
                Console.WriteLine(k); // 可以访问外部变量
                Console.WriteLine("This is DoNothing");
            };
        }
        {
            // .net 3.0 lambda
            // 左边是参数列表 goes to 右边方法体
            // 本质就是一个方法
            // 多播委托+=的lambda再-=同样的lambda也无法去掉
            NoReturnNoPara method = () =>// goes to
            {
                Console.WriteLine(k);// 可以访问外部变量
                Console.WriteLine("This is DoNothing");
            };
        }
    }

    private void DoNothing()
    {
        Console.WriteLine("This is DoNothing");
    }
}

public static class ExtendMethod
{
    // 扩展方法
    public static void Extend(this Student student)
    {
        Console.WriteLine(student.Name);
    }

    public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> source, Func<T, bool> func)
    {
        foreach (var item in source)
        {
            if (func.Invoke(item))
            {
                yield return item;
            }
        }
    }
}

public class LinqShow
{
    // LINQ
    // 1.基于委托封装解耦，去掉重复代码，完成通用代码
    // 2.泛型，应对各种数据类型
    // 3.加迭代器，按需获取
    public void Show()
    {
        var masters = new List<MartialArtsMaster>()
        {
            new MartialArtsMaster() {Id = 1,Name="黄蓉",Age=18,Menpai="丐帮",Kongfu="打狗棒法",Level=9},
            new MartialArtsMaster() {Id = 2,Name="洪七公",Age=70,Menpai="丐帮",Kongfu="打狗棒法",Level=10},
            new MartialArtsMaster() {Id = 3,Name="郭靖",Age=22,Menpai="丐帮",Kongfu="降龙十八掌",Level=10},
            new MartialArtsMaster() {Id = 4,Name="任我行",Age=50,Menpai="明教",Kongfu="葵花宝典",Level=1},
            new MartialArtsMaster() {Id = 5,Name="东方不败",Age=35,Menpai="明教",Kongfu="葵花宝典",Level=10},
            new MartialArtsMaster() {Id = 6,Name="林平之",Age=23,Menpai="华山",Kongfu="葵花宝典",Level=7},
            new MartialArtsMaster() {Id = 7,Name="岳不群",Age=50,Menpai="华山",Kongfu="葵花宝典",Level=8},
            new MartialArtsMaster() {Id = 8,Name="令狐冲",Age=23,Menpai="华山",Kongfu="独孤九剑",Level=10},
            new MartialArtsMaster() {Id = 9,Name="梅超风",Age=23,Menpai="桃花岛",Kongfu="九阴真经",Level=8},
            new MartialArtsMaster() {Id = 10,Name="黄药师",Age=23,Menpai="梅花岛",Kongfu="弹指神通",Level=10},
            new MartialArtsMaster() {Id = 11,Name="风清扬",Age=23,Menpai="华山",Kongfu="独孤九剑",Level=10},
        };
        var kongfus = new List<Kongfu>()
        {
            new Kongfu() {Id = 1,Name="打狗棒法",Power=90},
            new Kongfu() {Id = 2,Name="降龙十八掌",Power=95},
            new Kongfu() {Id = 3,Name="葵花宝典",Power=100},
            new Kongfu() {Id = 4,Name="独孤九剑",Power=100},
            new Kongfu() {Id = 5,Name="九阴真经",Power=100},
            new Kongfu() {Id = 6,Name="弹指神通",Power=100},
        };
        {
            // 集合过滤 这里保留等级大于8
            // 查询表达式
            var resE = from m in masters
                       where m.Level > 8
                       select m;
            // 扩展方法
            var resM = masters.Where(m => m.Level > 8);

        }
        {
            // 多条件过滤 这里保留等级大于8且门派为丐帮
            // 查询表达式
            var resE = from m in masters
                       where m.Level > 8 && m.Menpai == "丐帮"
                       select m;
            // 扩展方法
            var resM = masters.Where(m => m.Level > 8 && m.Menpai == "丐帮");
            foreach (var item in resE)
            {
                Console.WriteLine(item);
            }
        }
    }
}

public class MartialArtsMaster
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Menpai { get; set; }
    public string Kongfu { get; set; }
    public int Level { get; set; }
    public override string ToString()
    {
        return string.Format($"编号:{Id},名字:{Name},年龄:{Age},门派:{Menpai},功夫:{Kongfu},等级:{Level}");
    }
}
public class Kongfu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Power { get; set; }
    public override string ToString()
    {
        return string.Format($"编号:{Id},名字:{Name},威力:{Power}");
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        {
            Student student1 = new Student()
            {
                Id = 1,
                Name = "Test",
                Age = 28,
            };
            // 匿名类
            object student2 = new
            {
                Id = 1,
                Name = "Test",
                Age = 28,
            };
        }
        {
            // 扩展方法
            Student student = new Student();
            student.Extend();
        }
        {
            LinqShow linqShow = new LinqShow();
            linqShow.Show();
        }
    }
}