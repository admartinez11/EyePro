using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Optometrista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Dashboard.Optometrista
{
    internal class ControllerAddLens
    {
        ViewAddLens ObjAddLens;
        private int accion;

        public ControllerAddLens(ViewAddLens Vista, int accion, int lens_ID, int con_ID, string OD_esfera, string OD_cilindro, string OD_eje, string OD_prisma, string OD_adicion, string OI_esfera, string OI_cilindro, string OI_eje, string OI_prisma, string OI_adicion)
        {
            //Acciones Iniciales
            ObjAddLens = Vista;
            this.accion = accion;
            Vista.Load += new EventHandler(CargaInicial);
            //Métodos iniciales: estos metodos se ejecutan cuando el formulario está cargando
            verificarAccion();
            ChargeValues(lens_ID, con_ID, OD_esfera, OD_cilindro, OD_eje, OD_prisma, OD_adicion, OI_esfera, OI_cilindro, OI_eje, OI_prisma, OI_adicion);
            //Métodos que se ejecutan al ocurrir eventos
            ObjAddLens.btnGuardar.Click += new EventHandler(NewRegister);
            ObjAddLens.btnActualizar.Click += new EventHandler(UpdateRegister);

        }

        public ControllerAddLens(ViewAddLens frmAddLens, int accion)
        {
            ObjAddLens = frmAddLens;
            this.accion = accion;
            frmAddLens.Load += new EventHandler(CargaInicial);
            verificarAccion();
            ObjAddLens.btnActualizar.Click += new EventHandler(UpdateRegister);
            ObjAddLens.btnGuardar.Click += new EventHandler(NewRegister);
        }

        void CargaInicial(object sender, EventArgs e)
        {
            LlenarcomboCodigoArt();
        }

        void LlenarcomboCodigoArt()
        {
            DAOLens daoArt = new DAOLens();
            DataSet ds = daoArt.ObtenerConsulta();
            ObjAddLens.cbcon_ID.DataSource = ds.Tables["Consulta"];
            ObjAddLens.cbcon_ID.DisplayMember = "cli_DUI";
            ObjAddLens.cbcon_ID.ValueMember = "con_ID";
        }

        public void verificarAccion()
        {
            if (accion == 1)
            {
                ObjAddLens.btnGuardar.Enabled = true;
                ObjAddLens.btnActualizar.Enabled = false;
            }
            else if (accion == 2)
            {
                ObjAddLens.btnGuardar.Enabled = false;
                ObjAddLens.btnActualizar.Enabled = true;
            }
        }

        public void NewRegister(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                //Se crea una instancia de la clase DAOLens llamada DAOInsert
                DAOLens DAOInsert = new DAOLens();
                //Datos para creación de persona
                //OD
                DAOInsert.con_ID1 = int.Parse(ObjAddLens.cbcon_ID.SelectedValue.ToString());
                DAOInsert.OD_esfera1 = ObjAddLens.txtOIEsfera.Text.Trim();
                DAOInsert.OD_cilindro1 = ObjAddLens.txtOICilindro.Text.Trim();
                DAOInsert.OD_eje1 = ObjAddLens.txtOIEje.Text.Trim();
                DAOInsert.OD_prisma1 = ObjAddLens.txtOIPrisma.Text.Trim();
                DAOInsert.OD_adicion1 = ObjAddLens.txtOIAdicion.Text.Trim();
                //OI
                DAOInsert.OI_esfera1 = ObjAddLens.txtODEsfera.Text.Trim();
                DAOInsert.OI_cilindro1 = ObjAddLens.txtODCilindro.Text.Trim();
                DAOInsert.OI_eje1 = ObjAddLens.txtODEje.Text.Trim();
                DAOInsert.OI_prisma1 = ObjAddLens.txtODPrisma.Text.Trim();
                DAOInsert.OI_adicion1 = ObjAddLens.txtODAdicion.Text.Trim();

                int valorRetornado = DAOInsert.InsertarLens();

                //Se verifica el valor que retornó el metodo anterior y que fue almacenado en la variable valorRetornado
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido registrados exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjAddLens.Close();
                }
                else
                {
                    MessageBox.Show("EPV006 - Los datos no pudieron ser registrados",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            
        }

        public void UpdateRegister(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                DAOLens daoUpdate = new DAOLens();
                daoUpdate.lens_ID1 = int.Parse(ObjAddLens.txtDRid.Text.ToString());
                daoUpdate.con_ID1 = int.Parse(ObjAddLens.cbcon_ID.SelectedValue.ToString());
                daoUpdate.OD_esfera1 = ObjAddLens.txtOIEsfera.Text.Trim();
                daoUpdate.OD_cilindro1 = ObjAddLens.txtOICilindro.Text.Trim();
                daoUpdate.OD_eje1 = ObjAddLens.txtOIEje.Text.Trim();
                daoUpdate.OD_prisma1 = ObjAddLens.txtOIPrisma.Text.Trim();
                daoUpdate.OD_adicion1 = ObjAddLens.txtOIAdicion.Text.Trim();
                //OI
                daoUpdate.OI_esfera1 = ObjAddLens.txtODEsfera.Text.Trim();
                daoUpdate.OI_cilindro1 = ObjAddLens.txtODCilindro.Text.Trim();
                daoUpdate.OI_eje1 = ObjAddLens.txtODEje.Text.Trim();
                daoUpdate.OI_prisma1 = ObjAddLens.txtODPrisma.Text.Trim();
                daoUpdate.OI_adicion1 = ObjAddLens.txtODAdicion.Text.Trim();

                int valorRetornado = daoUpdate.ActualizarLens();
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido actualizado exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjAddLens.Close();
                }
                else if (valorRetornado == 2)
                {
                    MessageBox.Show("EPV002 -Los datos no pudieron ser actualizados completamente",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("EPV001 - Error inesperado",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidarCampos()
        {
            CommonClasses commonClasses = new CommonClasses();

            // Validación Ojo Derecho Esfera
            string OD_esfera = ObjAddLens.txtOIEsfera.Text.Trim();
            if (!string.IsNullOrEmpty(OD_esfera))
            {
                if (!Regex.IsMatch(OD_esfera, @"^\d+$"))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Derecho Esfera", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OD_esfera.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación Ojo Derecho Cilindro
            string OD_cilindro = ObjAddLens.txtOICilindro.Text.Trim();
            if (!string.IsNullOrEmpty(OD_cilindro))
            {
                if (!OD_cilindro.Contains("."))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Derecho Cilindro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                int dotCount = OD_cilindro.Split('.').Length - 1;
                if (dotCount != 1)
                {
                    MessageBox.Show("El tipo de valores ingresados es incorrecto. Debe contener un solo punto decimal.", "Validación de Ojo Derecho Cilindro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OD_cilindro.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación Ojo Derecho Eje
            string OD_eje = ObjAddLens.txtOIEje.Text.Trim();
            if (!string.IsNullOrEmpty(OD_eje))
            {
                if (!OD_eje.Contains("."))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Derecho Eje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                int dotCount1 = OD_eje.Split('.').Length - 1;
                if (dotCount1 != 1)
                {
                    MessageBox.Show("El tipo de valores ingresados es incorrecto. Debe contener un solo punto decimal.", "Validación de Ojo Derecho Eje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OD_eje.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación Ojo Derecho Prisma
            string OD_prisma = ObjAddLens.txtOIPrisma.Text.Trim();
            if (!string.IsNullOrEmpty(OD_prisma))
            {
                if (!Regex.IsMatch(OD_prisma, @"^\d+$"))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Derecho Prisma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OD_prisma.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación Ojo Derecho Adición
            string OD_adicion = ObjAddLens.txtOIPrisma.Text.Trim();
            if (!string.IsNullOrEmpty(OD_adicion))
            {
                if (!Regex.IsMatch(OD_adicion, @"^\d+$"))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Derecho Adición", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OD_adicion.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación Ojo Izquierdo Esfera
            string OI_esfera = ObjAddLens.txtODEsfera.Text.Trim();
            if (!string.IsNullOrEmpty(OI_esfera))
            {
                if (!Regex.IsMatch(OI_esfera, @"^\d+$"))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Izquierdo Esfera", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (OI_esfera.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            } 

            // Validación Ojo Izquierdo Cilindro
            string OI_cilindro = ObjAddLens.txtODCilindro.Text.Trim();
            if (!string.IsNullOrEmpty(OI_cilindro))
            {
                if (!OI_cilindro.Contains("."))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Izquierdo Cilindro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                int dotCount = OI_cilindro.Split('.').Length - 1;
                if (dotCount != 1)
                {
                    MessageBox.Show("El tipo de valores ingresados es incorrecto. Debe contener un solo punto decimal.", "Validación de Ojo Izquierdo Cilindro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OI_cilindro.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación Ojo Izquierdo Eje
            string OI_eje = ObjAddLens.txtODEje.Text.Trim();
            if (!string.IsNullOrEmpty(OI_eje))
            {
                if (!OI_eje.Contains("."))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Izquierdo Eje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                int dotCount1 = OI_eje.Split('.').Length - 1;
                if (dotCount1 != 1)
                {
                    MessageBox.Show("El tipo de valores ingresados es incorrecto. Debe contener un solo punto decimal.", "Validación de Ojo Izquierdo Eje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OI_eje.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación Ojo Izquierdo Prisma
            string OI_prisma = ObjAddLens.txtODPrisma.Text.Trim();
            if (!string.IsNullOrEmpty(OI_prisma))
            {
                if (!Regex.IsMatch(OI_prisma, @"^\d+$"))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Izquierdo Prisma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OI_prisma.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación Ojo Izquierdo Adición
            string OI_adicion = ObjAddLens.txtODAdicion.Text.Trim();
            if (!string.IsNullOrEmpty(OI_adicion))
            {
                if (!Regex.IsMatch(OI_adicion, @"^\d+$"))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Izquierdo Adición", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OD_adicion.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        public void ChargeValues(int lens_ID, int con_ID, string OD_esfera, string OD_cilindro, string OD_eje, string OD_prisma, string OD_adicion, string OI_esfera, string OI_cilindro, string OI_eje, string OI_prisma, string OI_adicion)
        {
            //Asigna los valores recibidos a los campos correpondientes a la vista ObjAddLens
            //Valores de Ojo Derecho
            ObjAddLens.txtDRid.Text = lens_ID.ToString();
            ObjAddLens.cbcon_ID.SelectedValue = lens_ID.ToString();
            ObjAddLens.txtOIEsfera.Text = OD_esfera;
            ObjAddLens.txtOICilindro.Text = OD_cilindro;
            ObjAddLens.txtOIEje.Text = OD_eje;
            ObjAddLens.txtOIPrisma.Text = OD_prisma;
            ObjAddLens.txtOIAdicion.Text = OD_adicion;
            //Valores de Ojo Izquierdo
            ObjAddLens.txtODEsfera.Text = OI_esfera;
            ObjAddLens.txtODCilindro.Text = OI_cilindro;
            ObjAddLens.txtODEje.Text = OI_eje;
            ObjAddLens.txtODPrisma.Text = OI_prisma;
            ObjAddLens.txtODAdicion.Text = OI_adicion;
        }
    }
}
