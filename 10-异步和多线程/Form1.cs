namespace _10_异步和多线程
{
    public partial class Form1 : Form
    {
        private TestClass _class = new TestClass();
        private int number = 0;

        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 同步方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(_class.Display("ClickIn"));
            DateTime start = DateTime.Now;
            Console.WriteLine("*return* " + _class.DoSomethingLong());
            _class.DoOtherthing();
            TimeSpan span = DateTime.Now - start;
            Console.WriteLine($"TimeSpan: {span.TotalSeconds}");
            Console.WriteLine(_class.Display("ClickOut"));
        }

        /// <summary>
        /// 异步方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine(_class.Display("ClickIn"));
            DateTime start = DateTime.Now;
            Async();
            TimeSpan span = DateTime.Now - start;
            Console.WriteLine($"TimeSpan: {span.TotalSeconds}");
            Console.WriteLine(_class.Display("ClickOut"));
        }

        private async void Async()
        {
            Console.WriteLine(_class.Display("AsyncIn"));
            string s = await Task.Run(_class.DoSomethingLong);
            Console.WriteLine("*return* " + s);
            string s2 = "*return* " + s + "  ***s2";
            Console.WriteLine(s2);
            string s3 = "*return* " + s2 + "  ***s3";
            Console.WriteLine(s3);
            Console.WriteLine(_class.Display("AsyncOut1"));
            Console.WriteLine(_class.Display("AsyncOut2"));
        }

        private void Write()
        {
            Console.WriteLine(_class.Display("ClickIn"));
            DateTime start = DateTime.Now;

            Action action = async () =>
            {
                Console.WriteLine(_class.Display("AsyncIn"));
                string s = await Task.Run(_class.DoSomethingLong);
                Console.WriteLine("*return* " + s);
                string s2 = "*return* " + s + "  ***s2";
                Console.WriteLine(s2);
                string s3 = "*return* " + s2 + "  ***s3";
                Console.WriteLine(s3);
                Console.WriteLine(_class.Display("AsyncOut1"));
                Console.WriteLine(_class.Display("AsyncOut2"));
            };
            action.Invoke();

            TimeSpan span = DateTime.Now - start;
            Console.WriteLine($"TimeSpan: {span.TotalSeconds}");
            Console.WriteLine(_class.Display("ClickOut"));
        }

        /// <summary>
        /// 异步方法+主线程长耗时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine(_class.Display("ClickIn"));
            DateTime start = DateTime.Now;
            Console.WriteLine("*return* " + Task.Run(_class.DoSomethingLong));
            //_class.DoOtherthing();
            TimeSpan span = DateTime.Now - start;
            Console.WriteLine($"TimeSpan: {span.TotalSeconds}");
            Console.WriteLine(_class.Display("ClickOut"));
        }

        /// <summary>
        /// Label数值+1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine(_class.Display("MainThreadDoing"));
            number++;
            label1.Text = number.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            number = 0;
            label1.Text = number.ToString();
        }
    }
}