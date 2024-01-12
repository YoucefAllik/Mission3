using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Mission3_GSB
{
    public partial class FrmVoirMedic : Form
    {
        private gsbrapportsEntities mesDonneesEF;

        public FrmVoirMedic(gsbrapportsEntities mesDonnees)
        {
            InitializeComponent();
            this.mesDonneesEF = mesDonnees;
            this.bdgSourceVMedic.DataSource = this.mesDonneesEF.Set<medicament>().ToList();

            // Check if 'offrir' is a DbSet in your context
            this.bdgSourceOffrir.DataSource = this.mesDonneesEF.Set<offrir>().ToList();

            this.bdgSourceFamille.DataSource = this.mesDonneesEF.Set<famille>().ToList();

        }


        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmUpdate update = new FrmUpdate(this.mesDonneesEF);
            update.Show();
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            FrmDelete del = new FrmDelete(this.mesDonneesEF);
            del.ShowDialog();
        }

        private void btnModif_Click(object sender, EventArgs e)
        {
            FrmUpdate update = new FrmUpdate(this.mesDonneesEF);
            update.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmAddMedic add = new FrmAddMedic(this.mesDonneesEF);
            add.ShowDialog();
        }

        private void btnAddFam_Click(object sender, EventArgs e)
        {
            FrmAddFamily addF = new FrmAddFamily(this.mesDonneesEF);
            addF.ShowDialog();
            this.cmBxFam.Update();
        }

        private void linqFilter()
        {
            var query = from m in mesDonneesEF.Set<medicament>()
                        where m.nomCommercial.StartsWith(txtFiltre.Text)
                        select m;
            bdgSourceVMedic.DataSource = query.ToList();

        }

        private void linqFamille()
        {
            var query = from m in mesDonneesEF.Set<medicament>()
                        where m.idFamille.StartsWith(cmBxFam.Text)
                        select m;
            bdgSourceVMedic.DataSource = query.ToList();

        }

        private string countMedic()
        {
            var query = (from m in mesDonneesEF.Set<medicament>()
                         where m.nomCommercial.StartsWith(txtFiltre.Text)
                         select m.id).Count();
            string result = query.ToString();
            return result;
        }

        private string countFam()
        {
            var query = (from m in mesDonneesEF.Set<medicament>()
                         where m.idFamille.StartsWith(cmBxFam.Text)
                         select m.id).Count();
            string result = query.ToString();
            return result;
        }

        private void txtFiltre_TextChanged(object sender, EventArgs e)
        {
            //lblError.Visible = false;

            if (txtFiltre.Text.Length >= 2)
            {
             
                linqFilter();
                dataGridView1.AutoResizeColumns();
               // lblError.Visible = false;
                lblError.Text = " ";
                lblCountResult.Text = "il y'a "+countMedic()+" trouvée";
            }
            else
            {
               // lblError.Visible = true;
                lblError.Text = "il faut mettre au moin les deux première lettre !!";
                
            }

          

        }

        private void cmBxFam_SelectedIndexChanged(object sender, EventArgs e)
        {
          

                linqFamille();
                dataGridView1.AutoResizeColumns();
            
                lblError.Text = " ";
                lblCountResult.Text = "il y'a " + countFam() + " trouvée";
            
          
        }

        private void btnXml_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Fichiers XML (*.xml)|*.xml";
            saveFileDialog.Title = "Enregistrer le fichier XML";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string cheminFichier = saveFileDialog.FileName;

                try
                {
                    // Création du document XML
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    xmlDoc.AppendChild(xmlDeclaration);

                    // Création de l'élément racine
                    XmlElement racineElement = xmlDoc.CreateElement("Medicaments");
                    xmlDoc.AppendChild(racineElement);

                    // Ajout des éléments pour chaque médicament dans la grille
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        XmlElement medicamentElement = xmlDoc.CreateElement("Medicament");

                        // Ajoutez les propriétés spécifiques de votre modèle de données
                        // Vous pouvez accéder aux cellules de la grille avec row.Cells[index]

                        // Exemple (assurez-vous d'ajuster les indices selon votre modèle de données):
                        XmlElement idFamilleElement = xmlDoc.CreateElement("IdFamille");

                        // Vérifiez si la valeur de la cellule n'est pas null avant de l'utiliser
                        if (row.Cells[0].Value != null)
                        {
                            idFamilleElement.InnerText = row.Cells[0].Value.ToString();
                        }
                        else
                        {
                            idFamilleElement.InnerText = string.Empty; // ou une autre valeur par défaut
                        }

                        medicamentElement.AppendChild(idFamilleElement);

                        XmlElement nomCommercialElement = xmlDoc.CreateElement("NomCommercial");

                        // Vérifiez si la valeur de la cellule n'est pas null avant de l'utiliser
                        if (row.Cells[1].Value != null)
                        {
                            nomCommercialElement.InnerText = row.Cells[1].Value.ToString();
                        }
                        else
                        {
                            nomCommercialElement.InnerText = string.Empty; // ou une autre valeur par défaut
                        }

                        medicamentElement.AppendChild(nomCommercialElement);

                        // Ajoutez d'autres propriétés du médicament selon vos besoins

                        racineElement.AppendChild(medicamentElement);
                    }

                    // Sauvegarde du fichier XML
                    xmlDoc.Save(cheminFichier);

                    MessageBox.Show("Exportation réussie vers " + cheminFichier, "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur s'est produite lors de la création du fichier XML : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}
