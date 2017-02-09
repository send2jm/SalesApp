using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace SalesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                displayStudy();     
        }
        private void Btn_Versions_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_Study.SelectedItem != null)
                DisplayCRFVersions();
        }

        public List<Study> studies;
        protected void displayStudy()
        {
            string url = "training1";
            if (!string.IsNullOrEmpty(txt_URL.Text))
                url = txt_URL.Text;
            studies = new List<Study>();
            StreamReader xmlText = connectHttpStream("https://" + url + ".mdsol.com/RaveWebServices/metadata/studies");
            if (xmlText != null)
            {

                XmlReader xmlReader = XmlReader.Create(xmlText.BaseStream);
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Study")
                    {
                        Study tempStudy = new Study(xmlReader.GetAttribute("OID"));
                        studies.Add(tempStudy);
                        //retOID += xmlReader.GetAttribute("OID") + Environment.NewLine;
                        /*
                        while (xmlReader.Read())
                        {
                            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "GlobalVariables")
                            {
                                while (xmlReader.Read())
                                {
                                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "ProtocolName")
                                    {
                                        retProtocol += xmlReader.ReadString() + Environment.NewLine; ;
                                        break;
                                    }
                                }
                            }
                        }*/

                    }
                }

                foreach (Study item in studies)
                {
                    ListBox_Study.Items.Add((item.mOID));
                }

               
            }

            
        }
        protected void DisplayCRFVersions()
        {
            string selected = ListBox_Study.SelectedValue.ToString();
            string url = "training1";
            if (!string.IsNullOrEmpty(txt_URL.Text))
                url = txt_URL.Text;


            StreamReader xmlText = connectHttpStream("https://" + url + ".mdsol.com/RaveWebServices/metadata/studies/"+ selected + "/drafts");
            string strxmlText = connectHttp("https://" + url + ".mdsol.com/RaveWebServices/metadata/studies/" + selected + "/drafts");


            MainWindow main = new MainWindow();
            
            //App.Current.MainWindow.Hide();

            //MessageBox.Show(strxmlText);
            ResultWindow test1 = new ResultWindow();
            test1.Show();
            test1.setText(strxmlText);

            //App.Current.MainWindow = main;
            //App.Current.MainWindow.Show();
            

        }
        protected string connectHttp(string actionUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(actionUrl);
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            request.ContentType = "text/xml";

            request.Method = "GET";

            string id= "jmseo";
            string pw = "password1";
            if (!string.IsNullOrEmpty(txt_ID.Text))
                id = txt_ID.Text;
            if (!string.IsNullOrEmpty(txt_PW.Password.ToString()))
                pw = txt_PW.Password.ToString();



            request.Credentials = new NetworkCredential(id, pw);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                ListBox_Study.Items.Clear();
                MessageBox.Show(e.Message.ToString(), "Incorrect Information");
                return null;
            }
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);


            return readStream.ReadToEnd();
        }
        protected StreamReader connectHttpStream(string actionUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(actionUrl);
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            request.ContentType = "text/xml";

            request.Method = "GET";
            string id = "jmseo";
            string pw = "password1";
            if (!string.IsNullOrEmpty(txt_ID.Text))
                id = txt_ID.Text;
            if (!string.IsNullOrEmpty(txt_PW.Password.ToString()))
                pw = txt_PW.Password.ToString();

            request.Credentials = new NetworkCredential(id, pw);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                
                ListBox_Study.Items.Clear();
                MessageBox.Show(e.Message.ToString(), "Incorrect Information");
                return null;
            }

            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);


            return readStream;
        }


        protected string FormatXml(string xmlString)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xmlString);

            StringBuilder sb = new StringBuilder();

            System.IO.TextWriter tr = new System.IO.StringWriter(sb);

            XmlTextWriter wr = new XmlTextWriter(tr);

            wr.Formatting = Formatting.Indented;

            doc.Save(wr);

            wr.Close();

            return sb.ToString();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DisplayCRFVersions();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void ListBox_Study_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox_Study.Items.Count > 0)
                Btn_Versions.IsEnabled = true;
        }
    }
}
