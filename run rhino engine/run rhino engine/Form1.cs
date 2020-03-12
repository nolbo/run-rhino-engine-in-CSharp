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
            rhino.setLanguageVersion(Context.VERSION_ES6); //버전을 ES6으로 지정
	    Script script;
            try
            {
                scope = new ImporterTopLevel(rhino);
                ScriptableObject.putProperty(scope, "ctx", this);
                ScriptableObject.defineClass(scope, new customMethod.Api().getClass()); //Api라는 클래스 추가
		/*컴파일
		
		script = rhino.compileReader(reader, "JavaScript", 1, null);
		resultBox.Text = script.exec(rhino, scope);
		*/
                resultBox.Text = rhino.evaluateString(scope, codeBox.Text, "JavaScript", 1, null).ToString(); //실행

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

            [JSStaticFunction] //자바에서는, @JSStaticFunction
            public static string Test()
            {
                return "Hello World!";
            }
        }
    }
}
