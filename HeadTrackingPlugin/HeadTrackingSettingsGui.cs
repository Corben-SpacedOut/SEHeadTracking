using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRageMath;

namespace HeadTrackingPlugin
{
    public class HeadTrackingSettingsGui : MyGuiScreenBase
    {
        private static readonly float HorizontalSpace = 25.0f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
        private static readonly float VerticalDelta = MyGuiConstants.CONTROLS_DELTA.Y;
        private static readonly float LabelOffsetX = -0.2f;

        public override string GetFriendlyName()
        {
            return "HeadTrackingSettingsGui";
        }

        public HeadTrackingSettingsGui() :
            base(position: new Vector2(0.5f, 0.5f),
                backgroundColor: MyGuiConstants.SCREEN_BACKGROUND_COLOR,
                size: new Vector2(0.5f, 0.7f),
                isTopMostScreen: false,
                backgroundTexture: null,
                backgroundTransition: MySandboxGame.Config.UIBkOpacity,
                guiTransition: MySandboxGame.Config.UIOpacity,
                gamepadSlot: null)
        {
            EnabledBackgroundFade = true;
            CloseButtonEnabled = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            RecreateControls(true);
        }

        public override void RecreateControls(bool constructor)
        {
            var settings = HeadTrackingSettings.Instance;

            MyGuiControlLabel caption = AddCaption(text: "Head Tracking Settings", captionOffset: new Vector2(0, 0.003f));

            float sepWidth = Size.Value.X * 0.8f;
            var index = 1;
            var baseY = caption.Position.Y;

            var separator = new MyGuiControlSeparatorList();
            separator.AddHorizontal(new Vector2(-sepWidth / 2, baseY + index * VerticalDelta), sepWidth);
            Controls.Add(separator);
            index++;

            var enabledCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(HorizontalSpace, baseY + index * VerticalDelta),
                    toolTip: "Enable/disable head tracking",
                    isChecked: settings.Enabled,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            enabledCheckbox.IsCheckedChanged += (box) => { settings.Enabled = box.IsChecked; };
            Controls.Add(enabledCheckbox);
            AddLabel(enabledCheckbox, "Head Tracking Enabled");
            index++;

            var inCharacterCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(HorizontalSpace, baseY + index * VerticalDelta),
                    toolTip: "Enable/disable head tracking when controlling character.",
                    isChecked: settings.EnabledInCharacter,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            inCharacterCheckbox.IsCheckedChanged += (box) => { settings.EnabledInCharacter = box.IsChecked; };
            Controls.Add(inCharacterCheckbox);
            AddLabel(inCharacterCheckbox, "Head Tracking In Character");
            index++;

            var inFpsCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(HorizontalSpace, baseY + index * VerticalDelta),
                    toolTip: "Enable/disable head tracking when controlling character in first person.",
                    isChecked: settings.EnabledInFirstPerson,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            inFpsCheckbox.IsCheckedChanged += (box) => { settings.EnabledInFirstPerson = box.IsChecked; };
            Controls.Add(inFpsCheckbox);
            AddLabel(inFpsCheckbox, "Head Tracking In FPS", offsetX: HorizontalSpace);
            index++;

            var invertPitchCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(HorizontalSpace, baseY + index * VerticalDelta),
                    toolTip: "Invert pitch axis.",
                    isChecked: settings.InvertPitch,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            invertPitchCheckbox.IsCheckedChanged += (box) => { settings.InvertPitch = box.IsChecked; };
            Controls.Add(invertPitchCheckbox);
            AddLabel(invertPitchCheckbox, "Invert Pitch");
            index++;

            var invertYawCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(HorizontalSpace, baseY + index * VerticalDelta),
                    toolTip: "Invert yaw axis.",
                    isChecked: settings.InvertYaw,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            invertYawCheckbox.IsCheckedChanged += (box) => { settings.InvertYaw = box.IsChecked; };
            Controls.Add(invertYawCheckbox);
            AddLabel(invertYawCheckbox, "Invert Yaw");
            index++;

            var invertRollCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(HorizontalSpace, baseY + index * VerticalDelta),
                    toolTip: "Invert roll axis.",
                    isChecked: settings.InvertRoll,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            invertRollCheckbox.IsCheckedChanged += (box) => { settings.InvertRoll = box.IsChecked; };
            Controls.Add(invertRollCheckbox);
            AddLabel(invertRollCheckbox, "Invert Roll");
            index++;

            var testModeCheckbox =
                new MyGuiControlCheckbox(
                    position: new Vector2(HorizontalSpace, baseY + index * VerticalDelta),
                    toolTip: "Test mode simulates head tracking.",
                    isChecked: settings.TestModeEnabled,
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            testModeCheckbox.IsCheckedChanged += (box) => { settings.TestModeEnabled = box.IsChecked; };
            Controls.Add(testModeCheckbox);
            AddLabel(testModeCheckbox, "Test Mode Enabled");
            index++;

            var separator2 = new MyGuiControlSeparatorList();
            separator2.AddHorizontal(new Vector2(-sepWidth / 2, baseY + index * VerticalDelta), sepWidth);
            Controls.Add(separator2);
            index++;

            var buttonOffsetX = HorizontalSpace;

            var okButton =
                new MyGuiControlButton(
                    position: new Vector2(-buttonOffsetX, baseY + index * VerticalDelta),
                    text: MyTexts.Get(MyCommonTexts.Ok),
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
                    onButtonClick: (_) => {
                        HeadTrackingSettings.Instance.Save();
                        CloseScreen();
                    });
            Controls.Add(okButton);

            var cancelButton =
                new MyGuiControlButton(
                    position: new Vector2(buttonOffsetX, baseY + index * VerticalDelta),
                    text: MyTexts.Get(MyCommonTexts.Cancel),
                    originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                    onButtonClick: (_) => {
                        CloseScreen();
                    });
            Controls.Add(cancelButton);

            Pack();
        }

        private void Pack()
        {
            Vector2 min = Controls.First().Position;
            Vector2 max = Controls.First().Position + Controls.First().Size;

            foreach (var c in Controls)
            {
                Log.Info("Control: " + c);

                var p = c.Position;
                var s = c.Size;

                min = Vector2.Min(min, p);
                max = Vector2.Max(max, p + s);
            }

            //m_size = max - min;
            //m_position = min;
        }

        private void AddLabel(MyGuiControlBase control, string text, float offsetX = 0.0f)
        {
            Controls.Add(new MyGuiControlLabel(
                position: control.Position + new Vector2(LabelOffsetX+offsetX, control.Size.Y / 2),
                text: text,
                originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
        }

        protected override void OnClosed()
        {
            HeadTrackingSettings.Reload();
        }
    }
}
