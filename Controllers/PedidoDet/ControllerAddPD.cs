using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Optometrista;
using OpticaMultivisual.Views.Dashboard.PedidoDet;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Controllers.Helper;

namespace OpticaMultivisual.Controllers.Dashboard.PedidoDetalle
{
    internal class ControllerAddPD
    {
        ViewAddPedidoDet ObjAddPD;
        public int accion;

        public ControllerAddPD(ViewAddPedidoDet frmAddDR, int accion)
        {
            ObjAddPD = frmAddDR;
            this.accion = accion;
            frmAddDR.Load += new EventHandler(CargaInicial);
            verificarAccion();
            //ObjAddPD.btnActualizar.Click += new EventHandler(UpdateRegister);
            ObjAddPD.btnAgregar.Click += new EventHandler(NewRegister);
        }

        //Actualizar shit
        public ControllerAddPD(ViewAddPedidoDet Vista, int accion, int pd_ID, int con_ID, DateTime pd_fpedido, DateTime pd_fprogramada, string art_codigo, int art_cant, string pd_obser, int pd_recetalab)
        {
            //Acciones Iniciales
            ObjAddPD = Vista;
            this.accion = accion;
            Vista.Load += new EventHandler(CargaInicial);
            //Métodos iniciales: estos metodos se ejecutan cuando el formulario está cargando
            verificarAccion();
            ChargeValues(pd_ID, con_ID, pd_fpedido, pd_fprogramada, art_codigo, art_cant, pd_obser, pd_recetalab);
            //Métodos que se ejecutan al ocurrir eventos
            //ObjAddPD.btnAgregar.Click += new EventHandler(NewRegister);
            ObjAddPD.btnActualizar.Click += new EventHandler(UpdateRegister);
        }


        void CargaInicial(object sender, EventArgs e)
        {
            LlenarcomboCodigoArt();
            LlenarcomboConsultaID();
        }

        void LlenarcomboCodigoArt()
        {
            DAOPedidoDet daoArt = new DAOPedidoDet();
            DataSet ds = daoArt.ObtenerArticulos();
            ObjAddPD.cbart_codigo.DataSource = ds.Tables["Articulo"];
            ObjAddPD.cbart_codigo.DisplayMember = "art_codigo";
            ObjAddPD.cbart_codigo.ValueMember = "art_codigo";
        }

        void LlenarcomboConsultaID()
        {
            DAOPedidoDet daoCon = new DAOPedidoDet();
            DataSet ds = daoCon.ObtenerConsulta();
            ObjAddPD.cbcon_ID.DataSource = ds.Tables["Consulta"];
            ObjAddPD.cbcon_ID.DisplayMember = "con_ID";
            ObjAddPD.cbcon_ID.ValueMember = "con_ID";
        }

        public void verificarAccion()
        {
            if (accion == 1)
            {
                ObjAddPD.btnAgregar.Enabled = true;
                ObjAddPD.btnActualizar.Enabled = false;
            }
            else if (accion == 2)
            {
                ObjAddPD.btnAgregar.Enabled = false;
                ObjAddPD.btnActualizar.Enabled = true;
            }
        }

        public void NewRegister(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                //Se crea una instancia de la clase DAOPedidoDet llamada DAOInsert
                DAOPedidoDet DAOInsert = new DAOPedidoDet();
                DAOInsert.con_ID1 = ObjAddPD.cbcon_ID.SelectedValue.ToString();
                DAOInsert.pd_fpedido1 = ObjAddPD.DTPpd_fpedido.Value;
                DAOInsert.pd_fprogramada1 = ObjAddPD.DTPpd_fprogramada.Value;
                DAOInsert.art_codigo1 = ObjAddPD.cbart_codigo.SelectedValue.ToString();
                DAOInsert.art_cant1 = int.Parse(ObjAddPD.txtart_cant.Text.ToString());
                if (ObjAddPD.cbpd_recetalab.Checked == true)
                {
                    DAOInsert.pd_recetalab1 = 1;
                }
                else
                {
                    DAOInsert.pd_recetalab1 = 0;
                }
                DAOInsert.pd_obser1 = ObjAddPD.txtpd_obser.Text.Trim();
                int valorRetornado = DAOInsert.InsertarPedido();
                //Se verifica el valor que retornó el método anterior y que fue almacenado en la variable valorRetornado
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido registrados exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjAddPD.Close();
                }
                else
                {
                    MessageBox.Show("EPV006 - No se pudieron registrar los datos",
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
                DAOPedidoDet daoUpdate = new DAOPedidoDet();

                daoUpdate.pd_ID1 = int.Parse(ObjAddPD.txtpd_ID.Text.ToString());
                daoUpdate.con_ID1 = ObjAddPD.cbcon_ID.SelectedValue.ToString();
                daoUpdate.pd_fpedido1 = ObjAddPD.DTPpd_fpedido.Value;
                daoUpdate.pd_fprogramada1 = ObjAddPD.DTPpd_fprogramada.Value;
                daoUpdate.art_codigo1 = ObjAddPD.cbart_codigo.SelectedValue.ToString();
                daoUpdate.art_cant1 = int.Parse(ObjAddPD.txtart_cant.Text.ToString());
                if (ObjAddPD.cbpd_recetalab.Checked == true)
                {
                    daoUpdate.pd_recetalab1 = 1;
                }
                else
                {
                    daoUpdate.pd_recetalab1 = 0;
                }
                daoUpdate.pd_obser1 = ObjAddPD.txtpd_obser.Text.Trim();
                int valorRetornado = daoUpdate.ActualizarPD();
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido actualizado exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjAddPD.Close();
                }
                else if (valorRetornado == 2)
                {
                    MessageBox.Show("EPV002 - Los datos no pudieron ser actualizados correctamente",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error: EPV001 - Error inesperado",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidarCampos()
        {
            CommonClasses commonClasses = new CommonClasses();

            if (ObjAddPD.cbcon_ID.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar una consulta.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (ObjAddPD.cbart_codigo.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un Código de Artículo.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ObjAddPD.txtart_cant.Text.Trim()))
            {
                MessageBox.Show("El campo de cantidad es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (ObjAddPD.txtart_cant.Text.Length > 40)
            {
                MessageBox.Show("El campo de Artículo Cantidad ha excedido el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string art_cant = ObjAddPD.txtart_cant.Text.Trim();
            if (!commonClasses.ValidarArtCant(art_cant))
            {
                MessageBox.Show("El campo de Artículo Cantidad contiene caracteres inválidos.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string pd_obser = ObjAddPD.txtpd_obser.Text.Trim();
            if (!string.IsNullOrEmpty(pd_obser))
            {
                if (!commonClasses.ObservacionValida(pd_obser))
                {
                    MessageBox.Show("La observación ingresada contiene caracteres inválidos", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (pd_obser.Length > 100)
                {
                    MessageBox.Show("Ha excedido el máximo de caracteres (100).",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }        

            DateTime fpedidodet = ObjAddPD.DTPpd_fpedido.Value.Date;
            DateTime fprogramada = ObjAddPD.DTPpd_fprogramada.Value.Date;
            if (!commonClasses.ValidarFechaPedido(fpedidodet, fprogramada))
            {
                MessageBox.Show("La fecha programada no es válida.",
                                "Error de validación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public void ChargeValues(int pd_ID, int con_ID, DateTime pd_fpedido, DateTime pd_fprogramada, string art_codigo, int art_cant, string pd_obser, int pd_recetalab)
        {
            ObjAddPD.txtpd_ID.Text = pd_ID.ToString();
            ObjAddPD.cbcon_ID.SelectedValue = con_ID;
            ObjAddPD.DTPpd_fpedido.Value = pd_fpedido;
            ObjAddPD.DTPpd_fpedido.Value = pd_fprogramada;
            ObjAddPD.cbart_codigo.SelectedValue = art_codigo;
            ObjAddPD.txtart_cant.Text = art_cant.ToString();
            ObjAddPD.txtpd_obser.Text = pd_obser;
            if (pd_recetalab == 1)
            {
                ObjAddPD.cbpd_recetalab.Checked = true;
            }
            else
            {
                ObjAddPD.cbpd_recetalab.Checked = false;
            }
        }
    }
}