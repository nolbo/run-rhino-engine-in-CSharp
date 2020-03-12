using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using org.mozilla.javascript;
using org.mozilla.javascript.annotations;

namespace run_rhino_engine
{
    public partial class RunningRhinoForm : Form
    {
        public RunningRhinoForm()
        {
            InitializeComponent();
        }

        Context rhino;
        Scriptable scope;
        private void runBtn_Click(object sender, EventArgs e)
        {
            rhino = Context.enter();
            rhino.setOptimizationLevel(-1);
            rhino.setLanguageVersion(Context.VERSION_ES6);
            try
            {
                scope = new ImporterTopLevel(rhino);
                ScriptableObject.putProperty(scope, "ctx", this);
                ScriptableObject.defineClass(scope, new customMethod.Api().getClass());
                resultBox.Text = rhino.evaluateString(scope, codeBox.Text, "JavaScript", 1, null).ToString();

            }
            catch(Exception error)
            {
                resultBox.Text = error.ToString();
            }
            finally
            {
                Context.exit();
            }
		}
    }

    public class customMethod
    {
        public class Api : ScriptableObject
        {
            public override string getClassName()
            {
                return "Api";
            }

            [JSStaticFunction]
            public static string Test()
            {
                return "Hello World!";
            }
        }
    }
}
