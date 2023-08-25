using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MotivatorEngine.PreTask.FeedbackChoice
{
    public class FeedbackHelper
    {
       private static string motivatorFolderPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"Motivator");
       private static string filename = Path.Join(motivatorFolderPath,"Feedbacks.json");
        private static FeedbackHelper helper;
       public static FeedbackHelper GetInstance()
        {
            if(helper == null)
            {
                helper = new FeedbackHelper();
            }
            return helper;
        }

        public static void ResetForTests()
        {
             filename = Path.Join(motivatorFolderPath, "FeedbacksTest.json");
            File.Delete(filename);
        }

    public List<Form> GetForms()
        {
            if(!Directory.Exists(motivatorFolderPath))
            {
                Directory.CreateDirectory(motivatorFolderPath);
            }
            if (!File.Exists(filename))
            {
                File.WriteAllText(filename, JsonConvert.SerializeObject(new List<Form>()));
            }
            return JsonConvert.DeserializeObject<List<Form>>(File.ReadAllText(filename), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public void CreateFileWithForms(List<Form> formList)
        {
            File.WriteAllText(filename, JsonConvert.SerializeObject(formList, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            }));
            Console.WriteLine("Saved feedback to : " + filename);
        }

        /// <summary>
        /// Add the form to the form list and save it to a new or existing form list file
        /// </summary>
        /// <param name="f"></param>
        public void AddAndSaveToFile(Form f)
        {

            List<Form> forms = GetForms();
            forms.Add(f);
            CreateFileWithForms(forms);
        }
    }
}
