using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Windows.Forms;

namespace System.CommandLine.StarFruit.WinForms
{
    public class GuiMaker
    {
        private static readonly int LeftMargin = 28;
        private static readonly int TopMargin = 50;
        private static readonly int LabelWidth = 300;
        private static readonly int Margin = 5;
        private static readonly int LabelHeight = 32;
        private static readonly int TextBoxHeight = 40;
        private static readonly int BottomMargin = 30;

        private readonly GuiForm form;
        private readonly ContainerControl parent;
        private readonly Control.ControlCollection controls;

        public GuiMaker()
        {
            form = new GuiForm();

            var panel = form.Controls
                      .OfType<Panel>()
                      .Where(p => p.Dock == DockStyle.Fill)
                      .FirstOrDefault();
            if (!(panel is null))
            {
                panel.AutoScroll = true;
                controls = panel.Controls;
            }
            else
            {
                controls = form.Controls;
            }
        }
        public void Show()
                => form.ShowDialog();

        public void Configure(InvocationContext context)
        {
            form.SetCommandText(context.ParseResult.TextToMatch());
            CommandResult commandResult = context.ParseResult.CommandResult;
            List<CommandResult> parentList = GetParentList(ref commandResult);
            var count = parentList.Count();
            for (int i = count - 1; i >= 0; i--)
            {
                Configure(parentList[i]);
            }
        }

        private static List<CommandResult> GetParentList(ref CommandResult commandResult)
        {
            var resultList = new List<CommandResult>() { commandResult };
            while (!(commandResult.Parent is null))
            {
                resultList.Add((CommandResult)commandResult.Parent);
                commandResult = (CommandResult)commandResult.Parent;
            }

            return resultList;
        }

        public void Configure(CommandResult result)
        {
            Configure(result.Command);
        }

        public void Configure(ICommand command)
        {
            var subCommands = command.Children
                                .OfType<Command>();
            var arguments = command.Arguments;
            var options = command.Options;
            var tabIndex = 6;
            form.SuspendLayout();
            GroupBox groupBox = MakeGroupBox(command, ref tabIndex);
            controls.Add(groupBox);
            groupBox.BringToFront();

            var top = TopMargin;
            foreach (var argument in arguments)
            {
                top += Margin;
                MakeTextBox(groupBox, argument.Name, top, EntryWidth(groupBox));
                top += TextBoxHeight;
            }
            foreach (var option in options)
            {
                top += Margin;
                groupBox.Controls.Add(MakeLabel(option.Name, top));
                Control control;
                if (((Argument)option.Argument).ArgumentType == typeof(bool))
                {
                    control = MakeCheckBox(groupBox, option.Name, top, EntryWidth(groupBox));
                }
                else if (((Argument)option.Argument).ArgumentType == typeof(int))
                {
                    control = MakeNumericBox(groupBox, option.Name, top, EntryWidth(groupBox));
                }
                else if (((Argument)option.Argument).ArgumentType == typeof(DateTime))
                {
                    control = MakeCalendar(groupBox, option.Name, top, EntryWidth(groupBox));
                }
                else
                {
                    control = MakeTextBox(groupBox, option.Name, top, EntryWidth(groupBox));
                }

                top += control.Height;
            }
            if (subCommands.Any())
            {
                top += Margin;

                ComboBox combo = MakeComboBox(groupBox, "Subcommands", top, EntryWidth(groupBox));
                combo.Items.AddRange(subCommands.Select(x => x.Name).ToArray());
                combo.SelectedIndexChanged += new EventHandler(ComboBox1_SelectedIndexChanged);
                top += combo.Height;
            }
            groupBox.Height = top + BottomMargin;
            form.ResumeLayout();
        }


        private static int EntryWidth(GroupBox groupBox)
            => groupBox.Width - LabelWidth - LeftMargin - 2 * Margin;

        private static Label MakeLabel(string text, int top)
            => new Label
            {
                Location = new Drawing.Point(LeftMargin, top),
                Text = text,
                Size = new Drawing.Size(LabelWidth, LabelHeight)
            };

        private static T AddControl<T>(Control parentControl, string labelText, int top, int width, Func<T> makeControl)
            where T : Control
        {
            Label label = MakeLabel(labelText, top);
            var left = label.Left + label.Width;
            var control = makeControl();
            control.Location = new Drawing.Point(left, top);
            control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            control.Size = new Drawing.Size(width, TextBoxHeight);

            parentControl.Controls.Add(label);
            parentControl.Controls.Add(control);
            return control;
        }

        private static ComboBox MakeComboBox(Control parentControl, string labelText, int top, int width)
         => AddControl(parentControl, labelText, top, width,
               () => new ComboBox());

        private static TextBox MakeTextBox(Control parentControl, string labelText, int top, int width)
            => AddControl(parentControl, labelText, top, width,
                () => new TextBox());

        private static CheckBox MakeCheckBox(Control parentControl, string labelText, int top, int width)
            => AddControl(parentControl, labelText, top, width,
                () => new CheckBox());

        private static NumericUpDown MakeNumericBox(Control parentControl, string labelText, int top, int width)
            => AddControl(parentControl, labelText, top, width,
                () => new NumericUpDown());

        private static MonthCalendar MakeCalendar(Control parentControl, string labelText, int top, int width)
            => AddControl(parentControl, labelText, top, width,
                () => new MonthCalendar());

        private static GroupBox MakeGroupBox(ICommand command, ref int tabIndex) => new GroupBox
        {
            Dock = DockStyle.Top,
            Name = $"groupBox{command.Name}",
            Size = new Drawing.Size(1242, 253),
            TabIndex = ++tabIndex,
            TabStop = false,
            Text = command.Name,
            Tag = command,
        };

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            var groupBox = comboBox.Parent as GroupBox;
            var command = groupBox.Tag as Command;
            var selected = comboBox.SelectedItem;
            var subCommand = command.Children
                                .OfType<Command>()
                                .Where(x => x.Name == (string)selected)
                                .First();
            Configure(subCommand);
        }

    }
}
