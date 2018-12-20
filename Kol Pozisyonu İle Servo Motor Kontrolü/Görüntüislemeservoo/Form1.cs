using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using AForge.Vision;
using AForge.Vision.Motion;
using AForge.Imaging;
using System.Drawing.Imaging;



namespace Görüntüislemeservoo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Color renk1;
        Bitmap image, image1, image2, p_1, p_2, p_3, islem;
        private FilterInfoCollection webcamsayisi; 
        private VideoCaptureDevice kamera;
        private void kullanilacakcihaz_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            image = (Bitmap)eventArgs.Frame.Clone();
            image1 = (Bitmap)eventArgs.Frame.Clone();
            image = new Mirror(false, true).Apply(image); // Ters hareketten dolayı aynalama yapıldı.
            image1 = new GrayscaleBT709().Apply(image); // Threshold uygulanabilmesi için gri'ye dönüştürüldü.
            image1 = new Threshold(120).Apply(image1);
            image1 = new Invert().Apply(image1); // Beyaz görüntü ile çalışmak istediğim için invert yapıldı.
            pictureBox2.Image = image1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webcamsayisi = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo videocapturedevice in webcamsayisi)
            {
                kameraBox.Items.Add(videocapturedevice.Name); // Kamera listesi 
            }
            kameraBox.SelectedIndex = 0;
          
            Control.CheckForIllegalCrossThreadCalls = false;
            for (int i = 0; i < System.IO.Ports.SerialPort.GetPortNames().Length; i++)
            { 
                comboBox1.Items.Add(System.IO.Ports.SerialPort.GetPortNames()[i]); // Mevcut portlar listelenir.
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            kamera = new VideoCaptureDevice(webcamsayisi[kameraBox.SelectedIndex].MonikerString);
            kamera.NewFrame += new NewFrameEventHandler(kullanilacakcihaz_NewFrame);
            kamera.Start(); // Kamera başlatılır.
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.Open();
                MessageBox.Show("Bağlantı Kuruldu.");
            }
            catch
            {
                MessageBox.Show("Bağlantı Kurulamadı.");
            }
            
        }     
        private void button4_Click(object sender, EventArgs e)
        {
            görüntü_box.Image = (Bitmap)pictureBox2.Image.Clone();  // Ana görüntü oluşturulur.
            p_3 = (Bitmap)görüntü_box.Image;
            serialPort1.Write("d"); // Motorların başlangıç pozisyonları sıfırlanır.
            timer4.Start();
        }    
        private void timer3_Tick(object sender, EventArgs e)
        {
            int gen = islem.Width;
            int yük = islem.Height;
            for (int y = 0; y < yük; y++) // Oluşan resmin piksellerinde dolaşılır.Ve İstenilen koşullar aranır.
            {
                for (int x = 0; x < gen; x++)
                {
                    renk1 = islem.GetPixel(x, y); 
                    if (renk1.R == 255 && renk1.G == 255 && renk1.B == 255 && y == 0 && x < 320)
                    {
                        label7.Text = "1. Motor Birinci pozisyon";
                        serialPort1.Write("1");
                    }
                    if (renk1.R == 255 && renk1.G == 255 && renk1.B == 255 && y == 0 && x > 320)
                    {
                        label4.Text = "2. Motor Birinci pozisyon";
                        serialPort1.Write("4");

                    }
                    if (renk1.R == 255 && renk1.G == 255 && renk1.B == 255 && y > 200 && y < 260 && x == 0)
                    {
                        label7.Text = "1. Motor İkinci pozisyon";
                        serialPort1.Write("2");
                      
                    }
                    if (renk1.R == 255 && renk1.G == 255 && renk1.B == 255 && y > 200 && y < 260 && x ==639)
                    {
                        label4.Text = "2. Motor ikinci pozisyon";
                        serialPort1.Write("5");
                    }
                    if (renk1.R == 255 && renk1.G == 255 && renk1.B == 255 && y > 440 && y < 480 && x==0 )
                    {
                        label7.Text = "1. Motor Üçüncü Pozisyon";
                        serialPort1.Write("3");
                    }
                    if (renk1.R == 255 && renk1.G == 255 && renk1.B == 255 && y > 440 && y < 480 && x == 639)
                    {
                        label4.Text = "2. Motor Üçüncü Pozisyon";
                        serialPort1.Write("6");
                    }
                }
            }
        }
        private void timer4_Tick(object sender, EventArgs e)
        {
            pictureBox3.Image = (Bitmap)pictureBox2.Image.Clone(); // Pozisyon görüntüsü oluşturulur.
            p_1 = (Bitmap)pictureBox3.Image;
            islem = new Subtract(p_3).Apply(p_1);  // Ana görüntüden pozisyon görüntüsü çıkartılır.Sadece pozisyon bilgisi elde edilir.
            islem = new FillHoles().Apply(islem); // Görüntüyü netleştirebilmek adına küçük boşluklar doldurulmuştur. Ve ardından opening işlemi yapılmıştır.
            islem = new Opening().Apply(islem);
            pictureBox5.Image = islem;
            timer3.Start();
        }
      
    }

    
}
