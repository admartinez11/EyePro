using OpticaMultivisual.Views.Dashboard.Optometrista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using OpticaMultivisual.Models.DAO;
using System.Windows.Forms;
using System.Security.Cryptography;
using OpticaMultivisual.Controllers.Helper;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace OpticaMultivisual.Controllers.Dashboard.Optometrista
{
    internal class ControllerAddDR
    {
        ViewAddDR ObjAddDR;
        private int accion;
        //pruebaaaa

        public ControllerAddDR(ViewAddDR Vista, int accion, int DR_ID, string con_ID, string OD_esfera, string OD_cilindro, string OD_eje, string OD_prisma, string OD_adicion, string OD_AO, string OD_AP, string OD_DP, string OI_esfera, string OI_cilindro, string OI_eje, string OI_prisma, string OI_adicion, string OI_AO, string OI_AP, string OI_DP)
        {
            //Acciones Iniciales
            ObjAddDR = Vista;
            this.accion = accion;
            Vista.Load += new EventHandler(CargaInicial);
            //Métodos iniciales: estos metodos se ejecutan cuando el formulario está cargando
            verificarAccion();
            ChargeValues(DR_ID, con_ID, OD_esfera, OD_cilindro, OD_eje, OD_prisma, OD_adicion, OD_AO, OD_AP, OD_DP, OI_esfera, OI_cilindro, OI_eje, OI_prisma, OI_adicion, OI_AO, OI_AP, OI_DP);
            //Métodos que se ejecutan al ocurrir eventos
            ObjAddDR.btnGuardar.Click += new EventHandler(NewRegister);
            ObjAddDR.btnActualizar.Click += new EventHandler(UpdateRegister);

        }

        public ControllerAddDR(ViewAddDR frmAddDR, int accion)
        {
            ObjAddDR = frmAddDR;
            this.accion = accion;
            frmAddDR.Load += new EventHandler(CargaInicial);
            verificarAccion();
            ObjAddDR.btnActualizar.Click += new EventHandler(UpdateRegister);
            ObjAddDR.btnGuardar.Click += new EventHandler(NewRegister);
        }

        void CargaInicial(object sender, EventArgs e)
        {
            LlenarcomboCodigoArt();
        }

        void LlenarcomboCodigoArt()
        {
            DAO_DR daoArt = new DAO_DR();
            DataSet ds = daoArt.ObtenerConsulta();
            ObjAddDR.cbcon_ID.DataSource = ds.Tables["Consulta"];
            ObjAddDR.cbcon_ID.DisplayMember = "cli_DUI";
            ObjAddDR.cbcon_ID.ValueMember = "con_ID";
        }

        public void verificarAccion()
        {
            if (accion == 1)
            {
                ObjAddDR.btnGuardar.Enabled = true;
                ObjAddDR.btnActualizar.Enabled = false;
            }
            else if (accion == 2)
            {
                ObjAddDR.btnGuardar.Enabled = false;
                ObjAddDR.btnActualizar.Enabled = true;
            }
        }

        public void NewRegister(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                //Se crea una instancia de la clase DAO_DR llamada DAOInsert
                DAO_DR DAOInsert = new DAO_DR();
                //Datos para creación de persona
                //OD
                //DAOInsert.DR_ID1 = int.Parse(ObjAddDR.txtDRid.Text.ToString());
                DAOInsert.OD_esfera1 = ObjAddDR.txtOIEsfera.Text.Trim();
                DAOInsert.con_ID1 = ObjAddDR.cbcon_ID.SelectedValue.ToString();
                DAOInsert.OD_cilindro1 = ObjAddDR.txtOICilindro.Text.Trim();
                DAOInsert.OD_eje1 = ObjAddDR.txtOIEje.Text.Trim();
                DAOInsert.OD_prisma1 = ObjAddDR.txtOIPrisma.Text.Trim();
                DAOInsert.OD_adicion1 = ObjAddDR.txtOIAdicion.Text.Trim();
                DAOInsert.OD_DP1 = ObjAddDR.txtOIdp.Text.Trim();
                DAOInsert.OD_AO1 = ObjAddDR.txtOIao.Text.Trim();
                DAOInsert.OD_AP1 = ObjAddDR.txtOIap.Text.Trim();
                //OI
                DAOInsert.OI_esfera1 = ObjAddDR.txtODEsfera.Text.Trim();
                DAOInsert.OI_cilindro1 = ObjAddDR.txtODCilindro.Text.Trim();
                DAOInsert.OI_eje1 = ObjAddDR.txtODEje.Text.Trim();
                DAOInsert.OI_prisma1 = ObjAddDR.txtODPrisma.Text.Trim();
                DAOInsert.OI_adicion1 = ObjAddDR.txtODAdicion.Text.Trim();
                DAOInsert.OI_DP1 = ObjAddDR.txtODdp.Text.Trim();
                DAOInsert.OI_AO1 = ObjAddDR.txtODao.Text.Trim();
                DAOInsert.OI_AP1 = ObjAddDR.txtODap.Text.Trim();

                int valorRetornado = DAOInsert.InsertarDR();
                //Se verifica el valor que retornó el método anterior y que fue almacenado en la variable valorRetornado
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido registrados exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjAddDR.Close();
                }
                else
                {
                    MessageBox.Show("EPV006 - Los datos no pudieron ser registrados",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("EPV006 - Los datos no pudieron ser registrados",
                                    "Proceso incompleto",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
        }

        public void UpdateRegister(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                DAO_DR daoUpdate = new DAO_DR();

                daoUpdate.DR_ID1 = int.Parse(ObjAddDR.txtDRid.Text.ToString());
                daoUpdate.con_ID1 = ObjAddDR.cbcon_ID.SelectedValue.ToString();
                daoUpdate.OD_esfera1 = ObjAddDR.txtOIEsfera.Text.Trim();
                daoUpdate.OD_cilindro1 = ObjAddDR.txtOICilindro.Text.ToString();
                daoUpdate.OD_eje1 = ObjAddDR.txtOIEje.Text.ToString();
                daoUpdate.OD_prisma1 = ObjAddDR.txtOIPrisma.Text.ToString();
                daoUpdate.OD_adicion1 = ObjAddDR.txtOIAdicion.Text.ToString();
                daoUpdate.OD_DP1 = ObjAddDR.txtOIdp.Text.ToString();
                daoUpdate.OD_AO1 = ObjAddDR.txtOIao.Text.ToString();
                daoUpdate.OD_AP1 = ObjAddDR.txtOIap.Text.ToString();
                //OI
                daoUpdate.OI_esfera1 = ObjAddDR.txtODEsfera.Text.Trim();
                daoUpdate.OI_cilindro1 = ObjAddDR.txtODCilindro.Text.ToString();
                daoUpdate.OI_eje1 = ObjAddDR.txtODEje.Text.ToString();
                daoUpdate.OI_prisma1 = ObjAddDR.txtODPrisma.Text.ToString();
                daoUpdate.OI_adicion1 = ObjAddDR.txtODAdicion.Text.ToString();
                daoUpdate.OI_DP1 = ObjAddDR.txtODdp.Text.ToString();
                daoUpdate.OI_AO1 = ObjAddDR.txtODao.Text.ToString();
                daoUpdate.OI_AP1 = ObjAddDR.txtODap.Text.ToString();

                int valorRetornado = daoUpdate.ActualizarDR();
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido actualizado exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjAddDR.Close();
                }
                else if (valorRetornado == 2)
                {
                    MessageBox.Show("EPV002 - Los datos no pudieron ser actualizados completamente",
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
            //OD
            string OD_esfera = ObjAddDR.txtOIEsfera.Text.Trim();
            string OD_cilindro = ObjAddDR.txtOICilindro.Text.Trim();
            string OD_eje = ObjAddDR.txtOIEje.Text.Trim();
            string OD_prisma = ObjAddDR.txtOIPrisma.Text.Trim();
            string OD_adicion = ObjAddDR.txtOIAdicion.Text.Trim();
            string OD_DP = ObjAddDR.txtOIdp.Text.Trim();
            string OD_AO = ObjAddDR.txtOIao.Text.Trim();
            string OD_AP = ObjAddDR.txtOIap.Text.Trim();
            //OI
            string OI_esfera = ObjAddDR.txtODEsfera.Text.Trim();
            string OI_cilindro = ObjAddDR.txtODCilindro.Text.Trim();
            string OI_eje = ObjAddDR.txtODEje.Text.Trim();
            string OI_prisma = ObjAddDR.txtODPrisma.Text.Trim();
            string OI_adicion = ObjAddDR.txtODAdicion.Text.Trim();
            string OI_DP = ObjAddDR.txtODdp.Text.Trim();
            string OI_AO = ObjAddDR.txtODao.Text.Trim();
            string OI_AP = ObjAddDR.txtODap.Text.Trim();

            // Validación de Ojo Derecho Esfera
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

            // Validación de Ojo Derecho Cilindro
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

            // Validación de Ojo Derecho Eje
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

            // Validación de Ojo Derecho Prisma
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

            // Validación de Ojo Derecho Adición
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

            // Validación de Ojo Derecho DP
            if (!string.IsNullOrEmpty(OD_DP))
            {
                if (!OD_DP.Contains("."))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Derecho DP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                int dotCount2 = OD_DP.Split('.').Length - 1;
                if (dotCount2 != 1)
                {
                    MessageBox.Show("El tipo de valores ingresados es incorrecto. Debe contener un solo punto decimal.", "Validación de Ojo Derecho DP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OD_DP.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación de Ojo Derecho AO
            if (!string.IsNullOrEmpty(OD_AO))
            {
                if (!Regex.IsMatch(OD_AO, @"^\d+$"))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Derecho AO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OD_AO.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación de Ojo Derecho AP
            if (!string.IsNullOrEmpty(OD_AP))
            {
                if (!OD_AP.Contains("."))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Derecho AP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                int dotCount3 = OD_AP.Split('.').Length - 1;
                if (dotCount3 != 1)
                {
                    MessageBox.Show("El tipo de valores ingresados es incorrecto. Debe contener un solo punto decimal.", "Validación de Ojo Derecho AP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OD_AP.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación de Ojo Izquierdo Esfera
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

            // Validación de Ojo Izquierdo Cilindro
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

            // Validación de Ojo Izquierdo Eje
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

            // Validación de Ojo Izquierdo Prisma
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

            // Validación de Ojo Izquierdo Adición
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

            // Validación de Ojo Izquierdo DP
            if (!string.IsNullOrEmpty(OI_DP))
            {
                if (!OI_DP.Contains("."))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Izquierdo DP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                int dotCount2 = OI_DP.Split('.').Length - 1;
                if (dotCount2 != 1)
                {
                    MessageBox.Show("El tipo de valores ingresados es incorrecto. Debe contener un solo punto decimal.", "Validación de Ojo Izquierdo DP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OD_DP.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Validación de Ojo Izquierdo AO
            if (!string.IsNullOrEmpty(OI_AO))
            {
                if (!Regex.IsMatch(OI_AO, @"^\d+$"))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Izquierdo AO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            // Validación de Ojo Izquierdo AP
            if (!string.IsNullOrEmpty(OI_AP))
            {
                if (!OI_AP.Contains("."))
                {
                    MessageBox.Show("El tipo de valores ingresados son incorrectos", "Validación de Ojo Izquierdo AP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                int dotCount3 = OI_AP.Split('.').Length - 1;
                if (dotCount3 != 1)
                {
                    MessageBox.Show("El tipo de valores ingresados es incorrecto. Debe contener un solo punto decimal.", "Validación de Ojo Izquierdo AP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (OD_AP.Length > 5)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (5).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        //finish validations

        public void ChargeValues(int DR_ID, string con_ID, string OD_esfera, string OD_cilindro, string OD_eje, string OD_prisma, string OD_adicion, string OD_AO, string OD_AP, string OD_DP, string OI_esfera, string OI_cilindro, string OI_eje, string OI_prisma, string OI_adicion, string OI_AO, string OI_AP, string OI_DP)
        {
            //Asigna los valores recibidos a los campos correspondientes a la vista ObjAddDR
            //Valores de Ojo Derecho
            ObjAddDR.txtDRid.Text = DR_ID.ToString();
            ObjAddDR.cbcon_ID.SelectedValue = con_ID.ToString();
            ObjAddDR.txtOIEsfera.Text = OD_esfera;
            ObjAddDR.txtOICilindro.Text = OD_cilindro;
            ObjAddDR.txtOIEje.Text = OD_eje;
            ObjAddDR.txtOIPrisma.Text = OD_prisma;
            ObjAddDR.txtOIAdicion.Text = OD_adicion;
            ObjAddDR.txtOIdp.Text = OD_DP;
            ObjAddDR.txtOIao.Text = OD_AO;
            ObjAddDR.txtOIap.Text = OD_AP;
            //Valores de Ojo Izquierdo
            ObjAddDR.txtODEsfera.Text = OI_esfera;
            ObjAddDR.txtODCilindro.Text = OI_cilindro;
            ObjAddDR.txtODEje.Text = OI_eje;
            ObjAddDR.txtODPrisma.Text = OI_prisma;
            ObjAddDR.txtODAdicion.Text = OI_adicion;
            ObjAddDR.txtODdp.Text = OI_DP;
            ObjAddDR.txtODao.Text = OI_AO;
            ObjAddDR.txtODap.Text = OI_AP;
        }
    }
}
