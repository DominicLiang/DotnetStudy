using System.Configuration;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

namespace _06_IO和序列化
{
    // 1.文件夹/文件 检查、新增、复制、移动、删除，递归编程技巧
    // 2.文件读写，记录文本日志，读取配置文件
    // 3.三种序列化器，xml和json
    // 4.验证码、图片缩放

    public class MyIO
    {
        // 配置文件绝对路径
        private static string LogPath = ConfigurationManager.AppSettings["LogPath"];
        private static string LogMovePath = ConfigurationManager.AppSettings["LogMovePath"];
        // 获取当前程序路径
        private static string LogPath2 = AppDomain.CurrentDomain.BaseDirectory;

        public static void Show()
        {
            {
                if (!Directory.Exists(LogPath))// 检测文件夹是否存在
                {
                    Console.WriteLine("文件夹不存在");
                }
                DirectoryInfo directory = new DirectoryInfo(LogPath);// 不存在不报错，有exists属性
                if (!File.Exists(Path.Combine(LogPath, "info.txt")))
                {
                }
                FileInfo fileInfo = new FileInfo(Path.Combine(LogPath, "info.txt"));
            }
            {
                // 文件夹 创建 移动 删除
                if (!Directory.Exists(LogPath))
                {
                    // 一次性创建所有子路径的文件夹
                    DirectoryInfo directory = Directory.CreateDirectory(LogPath);
                    // 移动文件夹 原文件夹没了
                    Directory.Move(LogPath, LogMovePath);
                    // 删除
                    Directory.Delete(LogMovePath);
                }
            }
            {
                string fileName = Path.Combine(LogPath, "log.txt");
                string fileNameCopy = Path.Combine(LogPath, "logCopy.txt");
                string fileNameMove = Path.Combine(LogPath, "logMove.txt");
                if (!File.Exists(fileName))// 确定文件是否存在
                {
                    Directory.CreateDirectory(LogPath);// 先创建文件夹才能创建里面的文件
                    // 打开文件流
                    using (FileStream fileStream = File.Create(fileName))// File.Create覆盖创建
                    {
                        string name = "1234567890";
                        byte[] bytes = Encoding.Default.GetBytes(name);
                        fileStream.Write(bytes, 0, bytes.Length);
                        fileStream.Flush();
                    }
                    using (FileStream fileStream = File.Create(fileName))
                    {
                        StreamWriter sw = new StreamWriter(fileStream);
                        sw.WriteLine("1234567890");
                        sw.Flush();
                    }
                    // 使用流写入器
                    using (StreamWriter sw = File.AppendText(fileName)) // File.AppendText添加
                    {
                        string msg = "1234567890";
                        sw.WriteLine(msg);
                        sw.Flush();
                    }
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        string name = "1234567890";
                        byte[] bytes = Encoding.Default.GetBytes(name);
                        sw.BaseStream.Write(bytes, 0, bytes.Length);
                        sw.Flush();
                    }

                    foreach (string result in File.ReadAllLines(fileName))
                    {
                        Console.WriteLine(result);
                    }
                    string sResult = File.ReadAllText(fileName);
                    Byte[] byteContent = File.ReadAllBytes(fileName);
                    string sResultByte = System.Text.Encoding.UTF8.GetString(byteContent);

                    using (FileStream stream = File.OpenRead(fileName))// 分批读取
                    {
                        int length = 5;
                        int result = 0;
                        do
                        {
                            byte[] bytes = new byte[length];
                            result = stream.Read(bytes, 0, 5);
                            for (int i = 0; i < result; i++)
                            {
                                Console.WriteLine(bytes[i].ToString());
                            }
                        } while (length == result);
                    }
                    File.Copy(fileName, fileNameCopy);
                    File.Copy(fileName, fileNameMove);
                    File.Delete(fileNameCopy);
                    File.Delete(fileNameMove);
                }
            }
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        Console.WriteLine(drive.Name + " is ready");
                    }
                    else
                    {
                        Console.WriteLine(drive.Name + " is not ready");
                    }
                }
            }
        }
    }



    public class SerializeHelper
    {
        public static void BinarySerialize()
        {
            string fileNameXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BinarySerialize.xml");
            string fileNameJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BinarySerialize.json");
            string fileNameBinary = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BinarySerialize.txt");
            List<Programmer> obj = DataFactory.BuildProgrammerList();

            {
                // 序列化 XML
                using (FileStream fs = File.Open(fileNameXML, FileMode.Create))
                {
                    DataContractSerializer s = new DataContractSerializer(typeof(List<Programmer>));
                    s.WriteObject(fs, obj);
                }
            }
            {
                // 反序列化 XML
                using (FileStream fs = File.Open(fileNameXML, FileMode.Open))
                {
                    DataContractSerializer s = new DataContractSerializer(typeof(List<Programmer>));
                    object? s2 = s.ReadObject(fs);
                    List<Programmer> result = s2 == null ? new List<Programmer>() : (List<Programmer>)s2;
                    foreach (var item in result)
                    {
                        Console.WriteLine(item.name);
                    }
                }
            }
            // JSON使用起来更加方便
            // 要序列化JSON有一点要注意，普通字段是不能被序列化的，必须用属性
            {
                // 序列化 JSON
                JsonSerializerOptions option = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(obj, option);
                File.WriteAllText(fileNameJson, jsonString);
                //异步序列化JSON
                //using (FileStream stream = File.Create(fileName))
                //{
                //    await JsonSerializer.SerializeAsync(stream, obj);
                //    await stream.DisposeAsync();
                //}
            }
            {
                // 反序列化 JSON
                string jsonString = File.ReadAllText(fileNameJson);
                List<Programmer> result = JsonSerializer.Deserialize<List<Programmer>>(jsonString)!;
                foreach (var item in result)
                {
                    Console.WriteLine(item.name);
                }
                //异步反序列化JSON
                //using (FileStream stream = File.OpenRead(fileName))
                //{
                //    object? obj = await JsonSerializer.DeserializeAsync<object>(stream);
                //}
            }
        }
    }

    public class DataFactory
    {
        public static List<Programmer> BuildProgrammerList()
        {
            List<Programmer> list = new List<Programmer>();
            list.Add(new Programmer()
            {
                Id = 1,
                description = "Vip",
                name = "SoWhat",
                sex = "男"
            });
            list.Add(new Programmer()
            {
                Id = 2,
                description = "Vip",
                name = "123",
                sex = "男"
            });
            list.Add(new Programmer()
            {
                Id = 3,
                description = "Vip",
                name = "Hello",
                sex = "女"
            });
            list.Add(new Programmer()
            {
                Id = 4,
                description = "Vip",
                name = "World",
                sex = "女"
            });
            return list;
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string name { get; set; } = string.Empty;
        public string sex { get; set; } = string.Empty;
    }

    public class Programmer : Person
    {
        public string description { get; set; } = string.Empty;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            {
                MyIO.Show();
            }
            {
                SerializeHelper.BinarySerialize();
            }
        }
    }
}