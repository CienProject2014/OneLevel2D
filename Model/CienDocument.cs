﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Navigation;
using Newtonsoft.Json;

namespace OneLevel2D.Model
{
    public class CienDocument
    {
        // static 변수는 자동으로 제외하기 때문에 이 attribute를 추가해줘야 한다.
        // static을 이렇게 남발해도 되는가? 필요하긴 한데, 필요를 없애는게 맞진 않은가?
        // set도 Serialization을 위해 public으로 두었다. 이렇게 해도 되는가?
        [JsonProperty]
        public static string ProjectDirectory { get; set; }
        [JsonProperty]
        public static string ExportDirectory { get; set; }
        [JsonProperty]
        public static string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<string> Resolutions { get; set; }
        public List<Asset> Assets { get; set; }
        public CienScene CurrentScene { get; set; }
        public List<CienScene> Scenes { get; set; }

        public const string DefaultLayerName = "Default";
        public const string PressedLayerName = "pressed";
        public const string NomalLayerName = "normal";

        public CienDocument()
        {
        }

        public void Init(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
            Resolutions = new List<string>(1) { "orig" };
            Assets = new List<Asset>();

            CurrentScene = new CienScene();
            CurrentScene.InitScene();
            Scenes = new List<CienScene>();
            Scenes.Add(CurrentScene);
        }

        #region Asset: Get, Add, Remove
        public Asset GetAsest(string assetName)
        {
            return Assets.Find(x => x.GetName() == assetName);
        }

        public void AddAsset(Asset asset)
        {
            // TODO assetList에 같은 이름의 asset이 있는지 중복 검사를 해야 한다.
            Asset newasseAsset = Assets.Find(x => x.GetName() == asset.GetName());
            if (newasseAsset != null)
            {
                MessageBox.Show(@"같은 이름의 Asset이 이미 프로젝트내에 존재합니다.");
                return;
            }
            Assets.Add(asset);

        }

        public void RemoveAsset(string name)
        {
            // 관련된 Component부터 다 지운다.
            List<string> removableList = new List<string>();

            foreach (var scene in Scenes)
            {
                foreach (var component in scene.Components)
                {
                    if (component is CienImage)
                    {
                        CienImage image = (CienImage)component;
                        image.ImageData.Dispose();
                        if (image.ImageName.Split('.')[0] == name)
                            removableList.Add(image.Id);
                    }
                    else if (component is CienComposite)
                    {
                        CienComposite composite = (CienComposite)component;
                        if (composite.composite.Images[0].ImageName.Split('.')[0] == name)
                            removableList.Add(composite.Id);
                    }
                }

                foreach (var removable in removableList)
                {
                    scene.Components.Remove(scene.Components.Find(x => x.Id.Equals(removable)));
                }
            }

            Assets.Remove(Assets.Find(x => x.GetName() == name));
        }
        #endregion

        #region Component: Add, Remove, Rename

        // TODO 무조건 이 함수를 통해서만 Component를 추가한다!
        public void AddComponent(CienComponent component)
        {
            // TODO Zindex를 정리해줄 필요가 있다.
            component.SetZindex(GetNewZindex());

            CurrentScene.Components.Add(component);

            CienComponent.Number++;
        }

        // Asset에서 만들어지는 Component는 무조건 이 함수를 통해서 만들어져야 한다.
        public void NewComponent(string name, Point location)
        {
            Asset asset = Assets.Find(x => x.GetName() == name);
            string id = "image" + CurrentScene.Components.Count;

            for(int i=CurrentScene.Components.Count; CurrentScene.Components.Find(x => x.Id == id) != null; i++)
            {
                id = Regex.Replace(id, @"[\d-]", "") + (i);
            }

            if (State.IsLayerSelected())
                AddComponent(new CienImage(asset.GetNameWithExt(), id, location, CienComponent.Number,
                    State.Selected.Layer.Name));
            else
                MessageBox.Show(@"선택된 layer가 없습니다!");
        }

        public void RemoveComponent(string id)
        {
            CurrentScene.Components.Remove(CurrentScene.Components.Find(x => x.Id == id));
        }

        public void RemoveComponent(CienComponent component)
        {
            RemoveComponent(component.Id);
        }

        #endregion

        public int GetNewZindex()
        {
            if (CurrentScene.Components.Count == 0) return 1;
            return CurrentScene.Components.Max(x => x.ZIndex) + 1;
        }

        public void SortComponentsAscending()
        {
            CurrentScene.Components.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));
        }
    }
}