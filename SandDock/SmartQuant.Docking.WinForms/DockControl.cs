using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using TD.SandDock;

namespace SmartQuant.Docking.WinForms
{
    public class ArrayKey
    {
        public object[] Keys { get; }

        public ArrayKey(params object[] keys)
        {
            Keys = keys;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ArrayKey))
                return base.Equals(obj);
            var other = (ArrayKey)obj;
            return Keys.Length == other.Keys.Length && !Keys.Where((t, i) => t != other.Keys[i]).Any();
        }

        public override int GetHashCode() => 0;
    }

    internal class DockControlKey
    {
        private readonly object _key;

        private static readonly object NullKey = new object();

        public DockControlKey(object key)
        {
            _key = key ?? NullKey;
        }

        public override bool Equals(object obj)
        {
            var key = obj as DockControlKey;
            return key != null ? _key.Equals(key._key) : base.Equals(obj);
        }

        public override int GetHashCode() => _key.GetHashCode();
    }

    public class DockControlSettings : IEnumerable<KeyValuePair<string, string>>
    {
        internal DockControlSettings()
        {
        }

        public void Clear() => _settings.Clear();

        public bool GetBooleanValue(string key, bool defaultValue)
        {
            var stringValue = GetStringValue(key, defaultValue.ToString(CultureInfo.InvariantCulture));
            bool result;
            return bool.TryParse(stringValue, out result) ? result : defaultValue;
        }

        public byte GetByteValue(string key, byte defaultValue)
        {
            var stringValue = GetStringValue(key, defaultValue.ToString(CultureInfo.InvariantCulture));
            byte result;
            return byte.TryParse(stringValue, NumberStyles.None, CultureInfo.InvariantCulture, out result)
                ? result
                : defaultValue;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _settings.GetEnumerator();

        public T GetEnumValue<T>(string key, T defaultValue) where T : struct
        {
            var stringValue = GetStringValue(key, defaultValue.ToString());
            T result;
            return Enum.TryParse(stringValue, out result) ? result : defaultValue;
        }

        public int[] GetInt32ArrayValue(string key, int[] defaultValue)
        {
            var stringValue = GetStringValue(key, null);
            if (string.IsNullOrWhiteSpace(stringValue))
                return defaultValue;
            var list = new List<int>();
            foreach (var s in stringValue.Split(','))
            {
                int item;
                if (!int.TryParse(s, out item))
                    return defaultValue;
                list.Add(item);
            }
            return list.ToArray();
        }

        public string GetStringValue(string key, string defaultValue)
        {
            string result;
            return _settings.TryGetValue(key, out result) ? result : defaultValue;
        }

        public TimeSpan GetTimeSpanValue(string key, TimeSpan defaultValue)
        {
            var stringValue = GetStringValue(key, defaultValue.ToString("c", CultureInfo.InvariantCulture));
            TimeSpan result;
            return TimeSpan.TryParse(stringValue, out result) ? result : defaultValue;
        }

        public void SetValue(string key, string value) => _settings[key] = value;

        public void SetValue(string key, byte value) => SetValue(key, value.ToString(CultureInfo.InvariantCulture));

        public void SetValue(string key, bool value) => SetValue(key, value.ToString(CultureInfo.InvariantCulture));

        public void SetValue(string key, TimeSpan value) => SetValue(key, value.ToString("c", CultureInfo.InvariantCulture));

        public void SetValue(string key, int[] value) => SetValue(key, string.Join(",", value.Select(num => num.ToString())));

        IEnumerator IEnumerable.GetEnumerator() => _settings.GetEnumerator();

        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();
    }

    [Serializable]
    public struct LayoutInfo
    {
        public string Layout;

        public LayoutLink[] Links;
    }

    [Serializable]
    public struct LayoutLink
    {
        public string TypeName;

        public Guid Guid;
    }

    [Serializable]
    public struct SettingsGroup
    {
        public string TypeName;

        public SettingsItem[] Items;
    }

    public struct SettingsInfo
    {
        public SettingsGroup[] Groups;
    }

    [Serializable]
    public struct SettingsItem
    {
        public string Key;

        public string Value;
    }

    public class DockManager : SandDockManager
    {
        public DockManager(Control dockSystemContainer, Form ownerForm)
        {
            DockSystemContainer = dockSystemContainer;
            OwnerForm = ownerForm;
        }

        public void CloseAll(bool documentsOnly)
        {
            foreach (var control in _controls)
            {
                if (documentsOnly)
                {
                    if (control.DockSituation == DockSituation.Document)
                        control.Close();
                    //if (control.LastDockedPosition == ContainerDockLocation.Center)
                    //{
                    //    control.Close();
                    //}
                }
                else
                    control.Close();
            }
        }

        public new DockControl[] GetDockControls() => base.GetDockControls().OfType<DockControl>().ToArray();

        public new LayoutInfo GetLayout()
        {
            var result = default(LayoutInfo);
            result.Links = (from current in _controls
                            where current.PersistState
                select new LayoutLink
                {
                    TypeName = $"{current.GetType().FullName}, {current.GetType().Assembly.GetName().Name}", Guid = current.Guid
                }).ToArray();
            result.Layout = base.GetLayout();
            return result;
        }

        public SettingsInfo GetSettings()
        {
            var result = default(SettingsInfo);
            var list = new List<SettingsGroup>();
            foreach (var settings in _settingsTable)
            {
                var item = default(SettingsGroup);
                item.TypeName = $"{settings.Key.FullName}, {settings.Key.Assembly.GetName().Name}";
                item.Items = settings.Value.Select(pair => new SettingsItem {Key = pair.Key, Value = pair.Value}).ToArray();
                list.Add(item);
            }
            result.Groups = list.ToArray();
            return result;
        }

        protected override void OnDockControlRemoved(DockControlEventArgs e)
        {
            var dockControl = e.DockControl as DockControl;
            if (dockControl != null)
            {
                var control = dockControl;
                _controls.Remove(control.GetType(), control.Key);
            }
            base.OnDockControlRemoved(e);
        }

        public DockControl Open(Type type) => Open(type, null);

        public DockControl Open(Type type, object key) => Open(type, key, true);

        public DockControl Open(Type type, object key, bool focus)
        {
            DockControl dockControl;
            if (_controls.TryGetControl(type, key, out dockControl))
                dockControl.Open();
            else
            {
                dockControl = (DockControl) Activator.CreateInstance(type);
                DockControlSettings settings;
                if (!_settingsTable.TryGetValue(type, out settings))
                {
                    settings = new DockControlSettings();
                    _settingsTable.Add(type, settings);
                }
                dockControl.Init(key, settings);
                dockControl.Manager = this;
                if (dockControl.ShowFloating)
                    dockControl.OpenFloating();
                else
                {
                    var container = FindDockContainer(dockControl.DefaultDockLocation);
                    var openMethod = focus ? WindowOpenMethod.OnScreenActivate : WindowOpenMethod.OnScreenSelect;
                    if (container == null)
                        dockControl.OpenDocked(dockControl.DefaultDockLocation, openMethod);
                    else
                    {
                        foreach (var control in GetDockControls().Where(c => c != dockControl && c.LayoutSystem.DockContainer == container))
                        {
                            dockControl.OpenWith(control);
                            dockControl.Open(openMethod);
                            break;
                        }
                    }
                }
                _controls.Add(type, key, dockControl);
            }
            return dockControl;
        }

        public void SetLayout(LayoutInfo layout)
        {
            var types = new Dictionary<Guid, Type>();
            foreach (var link in layout.Links)
            {
                var type = Type.GetType(link.TypeName, false);
                if (type != null)
                {
                    Open(type);
                    types.Add(link.Guid, type);
                }
            }

            ResolveDockControlEventHandler value = (sender, e) =>
            {
                Type t;
                DockControl dockControl;
                if (types.TryGetValue(e.Guid, out t) && _controls.TryGetControl(t, null, out dockControl))
                    e.DockControl = dockControl;
            };
            try
            {
                ResolveDockControl += value;
                SetLayout(layout.Layout);
            }
            finally
            {
                ResolveDockControl -= value;
            }
        }

        public void SetSettings(SettingsInfo settings)
        {
            _settingsTable.Clear();
            foreach (var group in settings.Groups)
            {
                var type = Type.GetType(group.TypeName, false);
                if (type != null)
                {
                    var s = new DockControlSettings();
                    _settingsTable.Add(type, s);
                    foreach (var item in group.Items)
                        s.SetValue(item.Key, item.Value);
                }
            }
        }

        private readonly DockControlWorkingSet _controls = new DockControlWorkingSet();

        private readonly Dictionary<Type, DockControlSettings> _settingsTable = new Dictionary<Type, DockControlSettings>();
    }
    
    [Designer("Design.UserDockControlDesigner", typeof(IDesigner)), Designer("Design.UserDockControlDocumentDesigner", typeof(IRootDesigner))]
    public class DockControl : TD.SandDock.DockControl
    {
        protected DockControl()
        {
            CloseAction = DockControlCloseAction.Dispose;
            DefaultDockLocation = ContainerDockLocation.Left;
            ShowFloating = false;
        }

        internal void Init(object key, DockControlSettings settings)
        {
            Key = key;
            Settings = settings;
            OnInit();
        }

        protected void InvokeAction(Action action)
        {
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        protected virtual void OnInit()
        {
            // noop
        }

        public override void Open() => Open(WindowOpenMethod.OnScreenActivate);

        protected override DockingRules CreateDockingRules()
        {
            throw new NotImplementedException();
        }

        [DefaultValue(DockControlCloseAction.Dispose)]
        public override DockControlCloseAction CloseAction
        {
            get
            {
                return base.CloseAction;
            }
            set
            {
                base.CloseAction = value;
            }
        }

        [Category("Docking"), DefaultValue(DEFAULT_DOCK_LOCATION)]
        public ContainerDockLocation DefaultDockLocation { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Key { get; private set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DockControlSettings Settings { get; private set; }

        [Category("Docking"), DefaultValue(DefaultShowFloating)]
        public bool ShowFloating { get; set; }

        private const ContainerDockLocation DEFAULT_DOCK_LOCATION = ContainerDockLocation.Left;

        private const bool DefaultShowFloating = false;
    }

    internal class DockControlWorkingSet : IEnumerable<DockControl>
    {
        public void Add(Type type, object key, DockControl control)
        {
            Dictionary<DockControlKey, DockControl> dictionary;
            if (!_table.TryGetValue(type, out dictionary))
            {
                dictionary = new Dictionary<DockControlKey, DockControl>();
                _table.Add(type, dictionary);
            }
            dictionary.Add(new DockControlKey(key), control);
        }

        public IEnumerator<DockControl> GetEnumerator() => _table.Values.SelectMany(d => d.Values).GetEnumerator();

        public void Remove(Type type, object key)
        {
            Dictionary<DockControlKey, DockControl> dictionary;
            if (!_table.TryGetValue(type, out dictionary))
                return;
            dictionary.Remove(new DockControlKey(key));
            if (dictionary.Count == 0)
                _table.Remove(type);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool TryGetControl(Type type, object key, out DockControl control)
        {
            Dictionary<DockControlKey, DockControl> dictionary;
            if (_table.TryGetValue(type, out dictionary))
                return dictionary.TryGetValue(new DockControlKey(key), out control);
            control = null;
            return false;
        }

        private readonly Dictionary<Type, Dictionary<DockControlKey, DockControl>> _table = new Dictionary<Type, Dictionary<DockControlKey, DockControl>>();
    }
}
