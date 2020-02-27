﻿#region License
/******************************************************************************
* Copyright 2019 The AutoCore Authors. All Rights Reserved.
* 
* Licensed under the GNU Lesser General Public License, Version 3.0 (the "License"); 
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
* 
* https://www.gnu.org/licenses/lgpl-3.0.html
* 
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*****************************************************************************/
#endregion


using System.Linq;

namespace AutoCore.MapToolbox.Autoware
{
    class ADASMapCrossWalk : ADASMapElement<ADASMapCrossWalk>
    {
        public int AID { get; set; }
        public enum Type : int
        {
            CLOSURE_LINE = 0,
            STRIPE_PATTERN = 1,
            BICYCLE_LANE = 2
        }
        public Type CrossWalkType { get; set; }
        public int BdID { get; set; }
        public int LinkID { get; set; }
        ADASMapArea area;
        public ADASMapArea Area
        {
            set => area = value;
            get
            {
                if (area == null && ADASMapArea.Dic.TryGetValue(AID, out ADASMapArea value))
                {
                    area = value;
                }
                return area;
            }
        }
        ADASMapLane linkLane;
        public ADASMapLane LinkLane
        {
            set => linkLane = value;
            get
            {
                if (linkLane == null && ADASMapLane.Dic.TryGetValue(LinkID, out ADASMapLane value))
                {
                    linkLane = value;
                }
                return linkLane;
            }
        }
        public override string ToString() => $"{ID},{Area.ID},{(int)CrossWalkType},{BdID},{LinkLane.ID}";
        const string file = "crosswalk.csv";
        const string header = "ID,AID,Type,BdID,LinkID";
        public static void ReadCsv(string path)
        {
            Clear();
            var data = Utils.ReadLinesExcludeFirstLine(path, file);
            if (data != null && data.Count() > 0)
            {
                foreach (var item in data.Split(','))
                {
                    new ADASMapCrossWalk
                    {
                        ID = int.Parse(item[0]),
                        AID = int.Parse(item[1]),
                        CrossWalkType = (Type)int.Parse(item[2]),
                        BdID = int.Parse(item[3]),
                        LinkID = int.Parse(item[4])
                    };
                }
            }
        }
        public static void PreWrite(string path)
        {
            Clear();
            Utils.CleanOrCreateNew(path, file, header);
        }
        public static void WriteCsv(string path)
        {
            ReIndex();
            Utils.AppendData(path, file, List.Select(_ => _.ToString()));
        }
    }
}