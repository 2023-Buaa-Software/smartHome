using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.Windows;
using System.Windows.Input;
using System.Speech.Synthesis;
using Smart_Home_App.UserControls;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Speech.Recognition;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Smart_Home_App
{
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer speechSyn;
        private SpeechRecognitionEngine speechRec;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSpeechRecognition();
            speechSyn = new SpeechSynthesizer();
            addDeviceButton.Add_Device += add_device;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool IsMaximize = false;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void InitializeSpeechRecognition()
        {
            // 初始化语音识别引擎，并设置为中文（中国）
            speechRec = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("zh-CN"));

            speechRec.SetInputToDefaultAudioDevice(); // 设置麦克风作为输入设备

            // 创建语法
            Choices commands = new Choices();
            string[] recognizeObj = { "打开厨房的冰箱", "打开厨房冰箱", "打开厨房空调", "打开厨房的空调", "打开温度计", "打开厨房的温度计", "打开厨房灯光", "打开厨房的灯光", "关闭厨房的冰箱", "关闭厨房冰箱",
                "关闭厨房空调", "关闭厨房的空调", "关闭温度计", "关闭厨房的温度计", "关闭厨房灯光", "关闭厨房的灯光" };

            commands.Add(recognizeObj);

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(commands);

            Grammar g = new Grammar(gb);

            speechRec.LoadGrammar(g);

            // 注册事件处理程序
            speechRec.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
        }

        private void microButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                speechRec.RecognizeAsync(RecognizeMode.Multiple); // 开始异步识别
                speechSyn.Speak("开始识别，请对着麦克风讲话。");
            }
            catch (InvalidOperationException ex)
            {
                speechSyn.Speak("启动识别时出错: " + ex.Message);
            }
        }

        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            speechSyn.Speak("收到指令" + e.Result.Text);

            string textToFind = e.Result.Text;
            string open_kitchen_sentence = "打开厨房的冰箱 打开厨房冰箱 打开厨房空调 打开厨房的空调 打开温度计 打开厨房的温度计 打开厨房灯光 打开厨房的灯光";
            string close_kitchen_sentence = "关闭厨房的冰箱 关闭厨房冰箱 关闭厨房空调 关闭厨房的空调 关闭温度计 关闭厨房的温度计 关闭厨房灯光 关闭厨房的灯光";

            // 将识别的文本显示在文本框中
            bool isKitchenOpen = open_kitchen_sentence.IndexOf(textToFind, StringComparison.OrdinalIgnoreCase) >= 0;
            bool isKitchenClose = close_kitchen_sentence.IndexOf(textToFind, StringComparison.OrdinalIgnoreCase) >= 0;

            string equipment = "冰箱空调温度计灯光";
            var commonChars = GetCommonCharacters(textToFind, equipment);
            string commonCharsString = new string(commonChars);

            if (isKitchenOpen && commonCharsString == "冰箱")
            {
                kitchen_Refridgerator.SetCheckBoxState(true);
                speechSyn.Speak("厨房的冰箱已经打开");
            }
            if (isKitchenOpen && commonCharsString == "空调")
            {
                kitchen_AirConditioner.SetCheckBoxState(true);
                speechSyn.Speak("厨房的空调已经打开");
            }
            if (isKitchenOpen && commonCharsString == "温度计")
            {
                kitchen_AirConditioner.SetCheckBoxState(true);
                speechSyn.Speak("厨房的温度计已经打开");
            }
            if (isKitchenOpen && commonCharsString == "灯光")
            {
                kitchen_Lights.SetCheckBoxState(true);
                speechSyn.Speak("厨房的灯光已经打开");
            }
            if (isKitchenClose && commonCharsString == "冰箱")
            {
                kitchen_Refridgerator.SetCheckBoxState(false);
                speechSyn.Speak("厨房的冰箱已经关闭");
            }
            if (isKitchenClose && commonCharsString == "空调")
            {
                kitchen_AirConditioner.SetCheckBoxState(false);
                speechSyn.Speak("厨房的空调已经关闭");
            }
            if (isKitchenClose && commonCharsString == "温度计")
            {
                kitchen_Temperature.SetCheckBoxState(false);
                speechSyn.Speak("厨房的温度计已经关闭");
            }
            if (isKitchenClose && commonCharsString == "灯光")
            {
                kitchen_Lights.SetCheckBoxState(false);
                speechSyn.Speak("厨房的灯光已经关闭");
            }
        }

        private async Task StartListeningAsync()
        {
            speechSyn.Speak("接收器打开");

            int port = 10000;
            string host = "192.168.124.8";

            ///创建终结点（EndPoint）
            IPAddress ip = IPAddress.Parse(host);//把ip地址字符串转换为IPAddress类型的实例
            IPEndPoint ipe = new IPEndPoint(ip, port);//用指定的端口和ip初始化IPEndPoint类的新实例

            ///创建socket并开始监听
            using (Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                listener.Bind(ipe);//绑定EndPoint对像（2000端口和ip地址）
                listener.Listen(2);//开始监听

                speechSyn.Speak("等待客户端连接");

                while (true)
                {
                    try
                    {
                        // 接受客户端连接，为此连接建立新的socket，并接受信息
                        Socket handler = await listener.AcceptAsync();
                        speechSyn.Speak("建立连接");

                        // 创建一个新任务来处理客户端连接，以便不阻塞主线程
                        // await HandleClientAsync(handler);
                        _ = Task.Run(() => HandleClientAsync(handler));

                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("SocketException: {0}", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: {0}", ex.Message);
                    }
                }
            }

        }

        private async Task HandleClientAsync(Socket handler)
        {
            byte[] buffer = new byte[1024];

            var eventArgs = new SocketAsyncEventArgs();
            eventArgs.SetBuffer(buffer, 0, buffer.Length);
            eventArgs.Completed += (sender, e) => ProcessReceive(e);

            try
            {
                while (true)
                {
                    eventArgs.SetBuffer(buffer, 0, buffer.Length);
                    bool willRaiseEvent = handler.ReceiveAsync(eventArgs);

                    // MessageBox.Show("willRaiseEvent");
                    Thread.Sleep(2000);
                    if (!willRaiseEvent)
                    {
                        ProcessReceive(eventArgs);
                    }

                    // await Task.Delay(10); // 适当的延迟，以防止过度的CPU使用
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
            finally
            {
                handler.Close();
            }
        }

        private async void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                string recvStr = Encoding.ASCII.GetString(e.Buffer, 0, e.BytesTransferred);
                // speechSyn.Speak($"{recvStr}");

                await Dispatcher.InvokeAsync(() => {
                    if (recvStr == "Touched" && kitchen_AirConditioner.GetCheckBoxState())
                    {
                        kitchen_AirConditioner.SetCheckBoxState(false);
                        speechSyn.Speak("关闭空调");
                    }
                    else if (recvStr == "Touched" && !kitchen_AirConditioner.GetCheckBoxState())
                    {
                        kitchen_AirConditioner.SetCheckBoxState(true);
                        speechSyn.Speak("打开空调");
                    }
                });
            }
        }

        static char[] GetCommonCharacters(string str1, string str2)
        {
            return str1.Intersect(str2).ToArray();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Task.Run(() => StartListeningAsync());
            //await StartListeningAsync();
        }

        private void add_device(object sender, Dictionary<string, string> e) {
            //
            ColumnDefinition columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(215);
            cards.ColumnDefinitions.Add(columnDefinition);
            Card card = new Card();
            card.Name = "card" + (cards.Children.Count + 1);
            card.Title = e["title"];
            card.Label = e["label"];
            card.IsHorizontal = true;
            card.ImageOff = card4.ImageOff;
            card.ImageOn = card4.ImageOn;
            //Grid.Column="3" IsHorizontal="True" Margin="12 0 12 0"
            Grid.SetColumn(card, cards.Children.Count);
            cards.Children.Add(card);
        }
    }
}