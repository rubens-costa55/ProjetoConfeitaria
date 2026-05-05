using System;
using System.Drawing;
using System.Windows.Forms;

namespace PrimeiraTela
{
    public static class MoverJanela
    {
        public static void Ativar(Form formulario, params Control[] controles)
        {
            bool arrastando = false;
            Point posicaoMouseInicial = Point.Empty;
            Point posicaoFormularioInicial = Point.Empty;

            void MouseDown(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    arrastando = true;
                    posicaoMouseInicial = Cursor.Position;
                    posicaoFormularioInicial = formulario.Location;
                }
            }

            void MouseMove(object sender, MouseEventArgs e)
            {
                if (arrastando)
                {
                    Point diferenca = Point.Subtract(Cursor.Position, new Size(posicaoMouseInicial));
                    formulario.Location = Point.Add(posicaoFormularioInicial, new Size(diferenca));
                }
            }

            void MouseUp(object sender, MouseEventArgs e)
            {
                arrastando = false;
            }

            formulario.MouseDown += MouseDown;
            formulario.MouseMove += MouseMove;
            formulario.MouseUp += MouseUp;

            foreach (Control controle in controles)
            {
                if (controle != null)
                {
                    controle.MouseDown += MouseDown;
                    controle.MouseMove += MouseMove;
                    controle.MouseUp += MouseUp;
                }
            }
        }
    }
}