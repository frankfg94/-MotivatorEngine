using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotivatorEngine.PreTask.FeedbackChoice;
using MotivatorPluginCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotivatorEngine.PreTask.FeedbackChoice.Tests
{
    [TestClass()]
    public class FeedbackHelperTests
    {

        [TestMethod()]
        public void TestFeedbackHelperSaveCorrectly()
        {
            // Mandatory
            FeedbackHelper.ResetForTests();

            Form f = Form.builder.AddQuestion("Test Question Title", "Test Question")
                        .SetTitle("Test title")
                        .AddYesNoQuestion("yes or no?")
                        .Build();

            Assert.AreEqual(0, FeedbackHelper.GetInstance().GetForms().Count) ;
            FeedbackHelper.GetInstance().AddAndSaveToFile(f);
            Assert.AreEqual(1, FeedbackHelper.GetInstance().GetForms().Count);

            Form f2 = Form.builder.AddQuestion("2nd Test Question Title", "2nd Test Question")
            .SetTitle("Test title 2")
            .AddYesNoQuestion("second form?")
            .Build();

           FeedbackHelper.GetInstance().AddAndSaveToFile(f2);
           Assert.AreEqual( 2, FeedbackHelper.GetInstance().GetForms().Count);
           TextFormData fd = (TextFormData) f.formDatas[0];
           TextFormData fd2 =  (TextFormData) FeedbackHelper.GetInstance().GetForms()[0].formDatas[0];
           Assert.AreEqual(fd.response,fd2.response);

        }
    }
}