using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Controllers.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.Login
{
    public partial class ViewLogin : Form
    {
        public ViewLogin()
        {
            InitializeComponent();
            //Inicializar controlador en la vista
            ControllerLogin control = new ControllerLogin(this);
            //Se elimina los bordes del formulario
            this.FormBorderStyle = FormBorderStyle.None;
            //Se invoca el método CreateRoundRectRgn contenido en la clase Helper
            Region = Region.FromHrgn(CommonClasses.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            //PasswordHide.Visible = false;
        }


    }
}
