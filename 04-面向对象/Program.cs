using System;

namespace _04_面向对象;

public abstract class Phone
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;

    public abstract void System();

    public void Call()
    {
        Console.WriteLine($"Use {GetType().Name} Call");
    }
    public virtual void Text()
    {
        Console.WriteLine($"Use {GetType().Name} Text");
    }
}

public class IPhone : Phone, IExtend
{
    public string Description { get; set; }
    public void Video()
    {
        Console.WriteLine("看视频。。。");
    }

    public int this[int index] { get => 1; set { } }

    public event Action ActionHandler;

    public override void System()
    {
        Console.WriteLine($"{GetType().Name} System is IOS");
    }

    public void Game(Game game)
    {
        game.Start();
        game.Play();
    }
}

public class XiaoMi : Phone
{
    public override void System()
    {
        Console.WriteLine($"{GetType().Name} System is MIUI");
    }

    public void Game(Game game)
    {
        game.Start();
        game.Play();
    }
}

public class Game
{
    public void Start() { }
    public void Play() { }
}

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    private int Salary { get; set; }

    public void UserPhone(Phone phone)
    {
        phone.Call();
        phone.Text();
        //phone.Video();
    }

    // 泛型+接口 扩展
    public void UserPhone<T>(T phone) where T : Phone, IExtend
    {
        phone.Call();
        phone.Text();
        phone.Video();
    }

    public void PlayIphone(IPhone iphone)
    {
        Game game = new Game();
        iphone.Game(game);
    }
}

public interface IExtend
{
    //string Remark;//no                   
    string Description { get; set; }
    void Video();
    //delegate void MyDelegate();//no
    //class MyClass//no
    //{
    //    string remark;
    //}
    event Action ActionHandler;
    int this[int index] { get; set; }
    // 接口不能定义字段、类、委托
    // 可以定义属性、方法、时间、索引器
}

public class Test
{
    public virtual void Call() { }
}

public class Test2:Test
{
    public override void Call() { }
}

public class Test3 : Test2
{
    // sealed之后不能再继续覆写
    public sealed override void Call() { }
}

public class Test4 : Test3
{
    // public override void Call() { }
}

public class Program
{
    static void Main()
    {
        // 面向过程
        Console.WriteLine("你得有一个手机");
        Console.WriteLine("开机。。。");
        Console.WriteLine("联网。。。");
        Console.WriteLine("启动游戏。。。");
        Console.WriteLine("一顿操作猛如虎。。。");
        Console.WriteLine("结束游戏。。。");
        // 业务不断的复杂下去
        // 面向对象
        Player player = new Player();
        player.Id = 1;
        player.Name = "Eleven";
        player.PlayIphone(new IPhone());
        // 面向对象的三大特性：1.封装 2.继承 3.多态（4.抽象）
        // 封装：
        // 数据安全；内部修改保持稳定；提供重用性；分工合作；职责分明
        // 方便构建大型复杂的系统
        // 继承：
        // 去掉重复代码；可以实现多态；
        // 侵入性很强的类关系
        // 多态：
        // 相同的变量，相同的操作，但是不同的实现
        // 方法的重载 接口&实现 抽象类&实现 继承
        // 抽象
        {
            Phone phone = new IPhone();
            phone.Call();
            phone.System();
            // phone.Video(); 
            // BasePhone没有，但是实例化的时候是有的
            // 是因为编译器限制了，实际在运行时是正确的
            dynamic dPhone = phone; //dynamic的作用是避开编译器的检查
            dPhone.Video();

            IExtend extend = new IPhone();
            extend.Video();
            dynamic dExtend = extend;
            dExtend.Call();

            // 怎么选择接口和抽象类
            // 接口：纯粹的约束      只能约束                 多实现更灵活   can do
            // 抽象类：父类+约束   可以完成通用实现    只能单继承      is a
            // 子类都一样的，放在父类；子类都有但是不同，抽象一下；有的有有的没有，那就是接口
            // 接口用的更多，因为接口更简单灵活 除非有些共有的需要继承
        }
    }
}