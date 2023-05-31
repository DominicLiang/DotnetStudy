Console.WriteLine("helloworld");

// 顶级语句 c# 9
// 一个项目只能有一个文件是使用顶级语句

// 全局using c# 10
// 只要项目里有一个文件用global using 那么这个using会应用到整个项目
// 一般会创造一个文件专门写global using
//global using System;

// using声明 c# 8
// 不再需要大括号了 当代码离开变量的作用域时会自动Dispose
// !!大括号可以强行创建作用域
//using StreamReader streamReader = new("file1.txt");

// 文件范围命名空间声明 c# 10
//namespace Test;

// 可空引用类型 c# 8
//string? s = "";

// init 访问器 c# 9
// 属性不用set而使用init的话只能在创建实例的时候赋值
//Dog dog = new Dog() { Id = 1, Name = "" };  √
//dog.Id = 1;                                 ×
//public class Dog
//{
//    public int Id { get; init; }
//    public string Name { get; init; }
//}

// 记录类型Record c# 9
// Record类型只要里面值相等那么==就是true
//Person p1 = new Person(1, "hello", 12);
//Person p2 = new Person(1, "hello", 12);
//Person p3 = new Person(2, "hello", 12);
//Console.WriteLine(p1);
//Console.WriteLine(p1 == p2);
//Console.WriteLine(p2 == p3);
//public record Person(int Id, string Name, int Age);

// switch表达式 c# 8
//string ToEnglish(int num) => num switch
//{
//    1 => One(),
//    2 => "two",
//    3 => "three",
//    4 => "four",
//    5 => "five",
//    _ => throw new NotImplementedException(),
//};
//string One()
//{
//    return "One";
//}