using System.Text;
using UnityEditor;

namespace Assets.Minamo.Editor {
    class Modifier_Identification : IModifier {
        readonly BuildTargetGroup targetGroup;

        // common
        string packageName;
        string versionName;
        // android = version code
        // ios : build number
        string versionCode;

        internal Modifier_Identification(BuildTargetGroup targetGroup) {
            this.targetGroup = targetGroup;
        }

        public void Reload(AnyDictionary dict) {
            packageName = dict.GetValue<string>("packageName");
            versionName = dict.GetValue<string>("versionName");
            versionCode = dict.GetValue<string>("versionCode");
        }

        internal static Modifier_Identification Current(BuildTargetGroup targetGroup) {
            string versionCode = "";
            if(targetGroup == BuildTargetGroup.Android) {
                versionCode = PlayerSettings.Android.bundleVersionCode.ToString();

            } else if(targetGroup == BuildTargetGroup.iOS) {
                versionCode = PlayerSettings.iOS.buildNumber;
            }

            return new Modifier_Identification(targetGroup)
            {
                packageName = PlayerSettings.GetApplicationIdentifier(targetGroup),
                versionName = PlayerSettings.bundleVersion,
                versionCode = versionCode,
            };
        }

        public void Apply() {
            PlayerSettings.SetApplicationIdentifier(targetGroup, packageName);
            PlayerSettings.bundleVersion = versionName;

            if(targetGroup == BuildTargetGroup.Android) {
                int parsed;
                if(!int.TryParse(versionCode, out parsed)) {
                    parsed = 0;
                }
                PlayerSettings.Android.bundleVersionCode = parsed;
            } else if(targetGroup == BuildTargetGroup.iOS) {
                PlayerSettings.iOS.buildNumber = versionCode;
            }
        }

        public string GetConfigText() {
            var sb = new StringBuilder();
            sb.AppendFormat("packageName={0}, ", packageName);
            sb.AppendFormat("versionName={0}, ", versionName);
            sb.AppendFormat("versionCode={0}, ", versionCode);
            return sb.ToString();
        }


    }
}
