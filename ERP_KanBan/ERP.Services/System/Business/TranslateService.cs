using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERP.Services.Business {
    public class TranslateService : BusinessService {

        JObject Translate;

        public TranslateService(
            UnitOfWork unitOfWork
        ) : base(unitOfWork) {
            Translate = Load("zh"); // defaut lang
        }

        public string Get(string key) {
            return Translate[key].ToString();
        }

        public void SetLang(string lang) {
            Translate = Load(lang);
        }
        private JObject Load(string lang) {
            
            string path = "lang/zh.json";
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("ERP.Services.dll", string.Empty);
            string frxFilePath = string.Format("{0}{1}", fileDirPath, path);

            
            if (File.Exists(string.Format("{0}{1}", fileDirPath, "lang/"+lang+".json")))
            {
                frxFilePath = string.Format("{0}{1}", fileDirPath, "lang/"+lang+".json");
            }

            string fileString = File.ReadAllText(frxFilePath);
            return JsonConvert.DeserializeObject<JObject>(fileString);
        }
       
    }
}