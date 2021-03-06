﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;


namespace NyaaBrowser
{
    //load config file and save data
    public class Data
    {
        private Client c;
        [JsonIgnore] public bool isSukebei;
        private Dictionary<string, string> CategoryDic;
        [JsonIgnore] public string[] normalCategories;
        [JsonIgnore] public string[] sukebeiCategories;
        [JsonProperty(PropertyName = "NormalUploaders")] public Dictionary<string, string> normalUploaderDic;
        [JsonProperty(PropertyName = "SukebeiUploaders")] public Dictionary<string, string> sukebeiUploaderDic;
        [JsonIgnore] public string[] normalUploaderStr;
        [JsonIgnore] public string[] sukebeiUploaderStr;
        [JsonProperty(PropertyName = "NormalTitles")] public Dictionary<string,string> normalTitlesDic;
        [JsonProperty(PropertyName = "SukebeiTitles")] public Dictionary<string, string> sukebeiTitlesDic;
        [JsonIgnore] public string[] normalTitlesStr;
        [JsonIgnore] public string[] sukebeiTitlesStr;


        private string fileName = "NyaaData.txt";
        [JsonProperty(PropertyName = "DownloadPath")] public string downloadPath = "D:\\Downloads";

        public Data(Client c)
        {
            this.c = c;
            isSukebei = false;
            //generate category dictionary
            CategoryDic = new Dictionary<string, string>();
            CategoryDic.Add("All categories", "0_0");
            CategoryDic.Add("Anime", "1_0");
            CategoryDic.Add("Anime - Anime Music Video", "1_32");
            CategoryDic.Add("Anime - English-translated Anime", "1_37");
            CategoryDic.Add("Anime - Non-English-translated Anime", "1_38");
            CategoryDic.Add("Anime - Raw Anime", "1_11");
            CategoryDic.Add("Audio", "3_0");
            CategoryDic.Add("Audio - Lossless Audio", "3_14");
            CategoryDic.Add("Audio - Lossy Audio", "3_15");
            CategoryDic.Add("Literature", "2_0");
            CategoryDic.Add("Literature - English-translated Literature", "2_12");
            CategoryDic.Add("Literature - Non-English-translated Literature", "2_39");
            CategoryDic.Add("Literature - Raw Literature", "2_13");
            CategoryDic.Add("Live Action", "5_0");
            CategoryDic.Add("Live Action - English-translated Live Action", "5_19");
            CategoryDic.Add("Live Action - Live Action Promotional Video", "5_22");
            CategoryDic.Add("Live Action - Non-English-translated Live Action", "5_21");
            CategoryDic.Add("Live Action - Raw Live Action", "5_22");
            CategoryDic.Add("Pictures", "4_0");
            CategoryDic.Add("Pictures - Graphics", "4_18");
            CategoryDic.Add("Pictures - Photos", "4_17");
            CategoryDic.Add("Software", "6_0");
            CategoryDic.Add("Software - Applications", "6_23");
            CategoryDic.Add("Software - Games", "6_24");
            CategoryDic.Add("Art", "7_0");
            CategoryDic.Add("Art - Anime", "7_25");
            CategoryDic.Add("Art - Doujinshi", "7_33");
            CategoryDic.Add("Art - Games", "7_27");
            CategoryDic.Add("Art - Pictures", "7_26");
            CategoryDic.Add("Real Life", "8_0");
            CategoryDic.Add("Real Life - Photobooks & Pictures", "8_31");
            CategoryDic.Add("Real Life - Videos", "8_30");

            //populate category codes
            normalCategories = new string[24];
            int index = 0;
            for (int i = 0; i < 24; i++)
            {
                normalCategories[i] = CategoryDic.ElementAt(index++).Key;
            }
            sukebeiCategories = new string[9];
            sukebeiCategories[0] = CategoryDic.ElementAt(0).Key;
            for (int i = 1; i < 9; i++)
            {
                sukebeiCategories[i] = CategoryDic.ElementAt(index++).Key;
            }

            //create dictionaties and populate by loading file
            normalUploaderDic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            sukebeiUploaderDic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            normalTitlesDic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            sukebeiTitlesDic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            LoadFile();
            
            UpdateAllStr();
            
            
        }

        public string GetCategoryCode(string category)
        {
            string result;
            CategoryDic.TryGetValue(category, out result);
            return result;    
       }

        public string GetUploaderID(string uploader, bool isSukebei)
        {
            string result;
            if (!isSukebei)
            {
                normalUploaderDic.TryGetValue(uploader, out result);
            }else
            {
                sukebeiUploaderDic.TryGetValue(uploader, out result);
            }
            
            return result;
        }

        public void SaveFile()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented, 
                new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore
                });
            File.WriteAllText(fileName, json);   
        }

        public void LoadFile()
        {
            string json = null;
            try
            {
                json = File.ReadAllText(fileName);
            }catch(IOException ioe)
            {
                c.mainScr.ShowMessage(ioe.ToString());
                
            }
            JsonConvert.PopulateObject(json, this);
            
        }

        //since the changes in dictionary can be unordered, it's impossible to simply resize the array
        public void UpdateSingleStr(ref Dictionary<string, string> dic, ref string[] str)
        {
            str = new string[dic.Count];
            for (int i = 0; i < dic.Count; i++)
            {
                str[i] = dic.ElementAt(i).Key;
            }
        }

        public void UpdateAllStr()
        {
            UpdateSingleStr(ref normalUploaderDic, ref normalUploaderStr);
            UpdateSingleStr(ref sukebeiUploaderDic, ref sukebeiUploaderStr);
            UpdateSingleStr(ref normalTitlesDic, ref normalTitlesStr);
            UpdateSingleStr(ref sukebeiTitlesDic, ref sukebeiTitlesStr);
        }


        
    }
}
