using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiParse0._2
{
    public class Program
    {
        private static string sNombre = string.Empty;
        private static string sAux = string.Empty;
        private static List<string> ListValuesLine = new List<string>();
        private static List<string> ListFinalValuesLine = new List<string>();
        //private static List<string> ListaLineasdeSalida = new List<string>();
        private static FicheroConfig fFicheroO;// Reglas para realizar el parse desde fichero datos libros hasta CSV final.

        public static void Main(string[] args)
        {
            //List<FicheroConfig> fFicheroO = new List<FicheroConfig>();// Reglas para realizar el parse desde fichero datos libros hasta CSV final.

            //StreamWriter pw = new StreamWriter(@"C:\juan\triston.txt");
            //pw.WriteLine("\"xkematic\",\"apple\",\"bq\"");
            //pw.Close();
            int iCountFirstline = 0;


            fFicheroO = new FicheroConfig(args[2]);
            //fFicheroO = new FicheroConfig("ConfigFile.MusiParse");

            fFicheroO.ReadConfigFile();
            //int iCont = 0, iStartItem = 0, iStopItem = 0;
            string lineFinal = string.Empty;
            StreamReader reader = new StreamReader(args[0]);
            StreamWriter writer = new StreamWriter(args[1]);

            //StreamReader reader = new StreamReader("Traspasomusikarte.txt");
            //StreamWriter writer = new StreamWriter("salida.txt");

            //StreamReader reader = new StreamReader(@"C:\juan\Dev\Musikarte\MusiParse0.2\Traspasomusikarte.txt");
            //StreamWriter writer = new StreamWriter(@"C:\juan\Dev\Musikarte\MusiParse0.2\PRUEBA.txt");

            while (!reader.EndOfStream)
            {
                if (iCountFirstline == 0)
                {
                    iCountFirstline++;
                    string sFirstLine = string.Empty;
                    for (int k = 0; k < fFicheroO.Listline.Count; k++)
                    {
                        if (k == fFicheroO.Listline.Count - 1)
                        {
                            sFirstLine = sFirstLine + fFicheroO.Listline[k].sNombreFinal;
                        }
                        else
                        {
                            sFirstLine = sFirstLine + fFicheroO.Listline[k].sNombreFinal + ",";
                        }
                    }
                    writer.WriteLine(sFirstLine);
                    //writer.WriteLine("referencia,nombre,idcategorias,marca,descripcion_corta,peso,largo,alto,titulo,autor,editorial,isbn,ancho,poferta,p1,p5,stockgen,imagen_web");
                }
                ReadDataLine(ref reader);
                ProcessItemforfinalFile();
                WritedFinalLine(writer ,ref lineFinal);
                //ListaLineasdeSalida.Add(lineFinal);
                ListValuesLine.Clear();
                ListFinalValuesLine.Clear();
            }
            writer.Close();
            //WritingFileResult();
        }

        private static void Review(ref string sItem, int iNumItem)
        {
            List<char> cCharactertoReplace = new List<char> { '/' , '&', '*', '+', '-' };

            char specific_character = '"';
            string specific_cadena = specific_character.ToString();

            for (int l = 0; l < cCharactertoReplace.Count; l++)
            {
                if (iNumItem == 15)
                { 
                    string cadena = cCharactertoReplace[l].ToString();
                    if (sItem.Contains(cadena))
                    {
                        int iIndexAux = sItem.ToList().FindIndex(n => n == cCharactertoReplace[l]);
                        if (sItem.ToList().Count == iIndexAux + 1)
                        {
                            sItem = sItem.Replace(cadena, ". ");
                        }
                        else if ((iIndexAux == 0) && (sItem.ToList()[iIndexAux + 1] != ' '))
                        {
                            sItem = sItem.Replace(cadena, " ");
                        }
                        else { 
                            if ((sItem.ToList()[iIndexAux-1]==' ') && (sItem.ToList()[iIndexAux + 1] != ' '))
                                sItem = sItem.Replace(cadena, "/ ");
                            else if ((sItem.ToList()[iIndexAux - 1] != ' ') && (sItem.ToList()[iIndexAux + 1] == ' '))
                                sItem = sItem.Replace(cadena, " /");
                            else if ((sItem.ToList()[iIndexAux - 1] != ' ') && (sItem.ToList()[iIndexAux + 1] != ' '))
                                sItem = sItem.Replace(cadena, " / ");
                            else if ((sItem.ToList()[iIndexAux - 1] == ' ') && (sItem.ToList()[iIndexAux + 1] == ' '))
                                sItem = sItem.Replace(cadena, "/");
                        }
                    }
                }
                if (sItem.Contains(specific_cadena))
                    sItem = sItem.Replace(specific_cadena, "&quot;");
            }

        }

        private static void ReadDataLine(ref StreamReader reader)
        {
            int iCont = 0, iStartItem = 0, iStopItem = 0;
            string lineOriginalFile;
            lineOriginalFile = reader.ReadLine();
            int ilongitem = 0;
            List<char> ListLine = new List<char>();
            ListLine = lineOriginalFile.ToList();

            iCont++; //para contar el numero de lineas del fichero original
            List<char> AuxLine = ListLine;
            int iContItemInALine = 0;
            while (iStartItem < ListLine.Count)
            {
                iStartItem = AuxLine.FindIndex(n => n != '&');
                //int icountuppersand = AuxLine.FindAll(n => n == '&').Count;
                //AuxLine[iStartItem + 1] == '&';
                string sItem = string.Empty;
                if (iStartItem != -1) //cuando iStartItem es igual a menor significa que ya no hay más campos con contenidos
                {
                    int iNumItem = iStartItem / 3;
                    for (int k = 0; k < iNumItem - 1; k++)
                    {
                        iContItemInALine++;
                        sItem = string.Empty;
                        ListValuesLine.Add(sItem);
                       // ProcessItemforfinalFile(iContItemInALine, ref sItem, ref lineFinal);
                    }
                    iContItemInALine++;
                    AuxLine = AuxLine.GetRange(iStartItem, AuxLine.Count - iStartItem);
                    ilongitem = AuxLine.FindIndex(n => n == '&');
                    if (AuxLine[ilongitem + 1] != '&')// && (AuxLine[ilongitem + 2] == '&'))
                    { 
                        //This is only for take into accoutn and & in the middle of Item
                        uppersandinthemiddle(AuxLine, ref ilongitem);
                    }
                    iStopItem = iStartItem + ilongitem;
                    sItem = string.Empty;
                    sItem = string.Concat(AuxLine.GetRange(0, (iStopItem - iStartItem)));
                    AuxLine = AuxLine.GetRange(ilongitem, AuxLine.Count - ilongitem);
                    sItem = sItem.TrimEnd(' ');
                    Review(ref sItem, iContItemInALine);
                    ListValuesLine.Add(sItem);
                }
                else// aqui sólo accedemos si a la línea sólo le quedan item vacíos
                {
                    int iNumEmptyItem = AuxLine.Count / 3;
                    for (int k = 0; k < iNumEmptyItem - 1; k++)
                    {
                        sItem = string.Empty;
                        iContItemInALine++;
                        ListValuesLine.Add(sItem);
                        // ProcessItemforfinalFile(iContItemInALine, ref sItem, ref lineFinal);
                    }
                    iStartItem = ListLine.Count + 1;// para forzar la salida del while de cada línea pq ya hemos terminado
                }
            }
            iStartItem = 0;
            iStopItem = 0;
            iContItemInALine = 0;
            //no estamos procesando correctamente, falta recortar la linea original ademas queremos escribir directamente en la estructura de salida no en una línea


        }

        private static void uppersandinthemiddle(List<char> AuxLine, ref int ilongitem)
        {
            List<char> Aux2 = AuxLine;
            int iStop = 0,iNewLongItem = ilongitem + 1;
            bool bEndItem = false;

            ilongitem = ilongitem + 1;
            iStop = ilongitem;
            Aux2 = Aux2.GetRange(ilongitem, Aux2.Count - ilongitem);
            while (!bEndItem)
            {
                ilongitem = Aux2.FindIndex(n => n == '&');

                if ((Aux2[ilongitem + 1] == '&') && (Aux2[ilongitem + 2] == '&'))
                {
                    bEndItem = true;
                    ilongitem = ilongitem + iNewLongItem;
                }
                else
                {

                    ilongitem = ilongitem + 1;
                    iNewLongItem = ilongitem + iNewLongItem;
                }
                Aux2 = Aux2.GetRange(ilongitem, Aux2.Count - ilongitem);
                //ilongitem++;
            }
        }

        private static void WritedFinalLine(StreamWriter writer, ref string lineFinal)
        {
            for (int k = 0; k < ListFinalValuesLine.Count; k++)
            {
                //writer.WriteLine(ListFinalValuesLine[k]);    medio funciona
                //lineFinal.Insert(0, ListFinalValuesLine[k]);// = lineFinal + ;
                if (fFicheroO.Listline[k].sIsString == "T")
                { 
                    if (k == 0)
                    {
                        lineFinal = "\"" + ListFinalValuesLine[k] + "\"" + ",";
                    } 
                    else if (k == (ListFinalValuesLine.Count - 1))
                    {
                        lineFinal = lineFinal + "\"" + ListFinalValuesLine[k] + "\"";
                    }
                    else
                    {
                        lineFinal = lineFinal + "\"" + ListFinalValuesLine[k] + "\"" + ",";
                    }
                }
                else if (fFicheroO.Listline[k].sIsString == "F")
                {
                    ListFinalValuesLine[k] = ListFinalValuesLine[k].Replace(',', '.');
                    if (k == 0)
                    {
                        lineFinal =  ListFinalValuesLine[k] +  ",";
                    }
                    else if (k == (ListFinalValuesLine.Count - 1))
                    {
                        lineFinal = lineFinal + ListFinalValuesLine[k] ;
                    }
                    else
                    {
                        lineFinal = lineFinal + ListFinalValuesLine[k]  + ",";
                    }
                }

            }
            writer.WriteLine(lineFinal);
          
        }

        private static void ProcessItemforfinalFile()
        {

            for (int i = 0; i < fFicheroO.iNumTotalItemsFinal; i++)
            {
                ListFinalValuesLine.Add(string.Empty);
            }

            for (int i = 0; i < fFicheroO.Listline.Count; i++)
            {
                //if (fFicheroO.Listline[i].sNombreFinal == "AUTOR")
                //{
                //    List<char> cCharactertoReplace = new List<char> { '&', '*', '+', '-'};
            
                //    for (int l=0; l < cCharactertoReplace.Count; l++)
                //    {
                     
                //        string cadena = cCharactertoReplace[l].ToString();
                //        ListValuesLine[fFicheroO.Listline[i].iPosOrigen[0] - 1] = ListValuesLine[fFicheroO.Listline[i].iPosOrigen[0] - 1].Replace(cadena, " / ");
                //    }
                //}
                if (fFicheroO.Listline[i].sTypeMove == "S")
                {
                    //SimpleLineCopy();
                    if (fFicheroO.Listline[i].sIsString == "F")
                    {
                        string sValue = ListValuesLine[fFicheroO.Listline[i].iPosOrigen[0] - 1];//.Replace(',', '.');
                        //double dValue = double.Parse(sValue);
                        ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] = sValue;
                    }
                    else
                    { 
                        ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] = ListValuesLine[fFicheroO.Listline[i].iPosOrigen[0] - 1];
                    }
                }
                else if (fFicheroO.Listline[i].sTypeMove == null)
                {
                    ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] = fFicheroO.Listline[i].sFinalValue;
                }
                else if (fFicheroO.Listline[i].sTypeMove == "MUL")
                {
                    double iAux = fFicheroO.Listline[i].dValueOperation;
                    iAux = iAux * double.Parse(ListValuesLine[fFicheroO.Listline[i].iPosOrigen[0] - 1]);
                    iAux = Math.Round(iAux, 2);
                  
                    double iAux2 = (iAux*100);
                    int resto = 0;
                    iAux2 = Math.DivRem((int)iAux2, 5, out resto);

                    if (resto != 0)
                    {
                        iAux = ((iAux2 * 5) + 5) / 100;
                    }
      
                    ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] = iAux.ToString();

                }
                else if (fFicheroO.Listline[i].sTypeMove == "PRE")
                {
                    ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] = fFicheroO.Listline[i].sPrefix +  ListValuesLine[fFicheroO.Listline[i].iPosOrigen[0] - 1];
                }
                else if (fFicheroO.Listline[i].sTypeMove == "M")
                {
                    int iAux = fFicheroO.Listline[i].iPosOrigen.Count;
                    for (int k = 0; k < iAux; k++)
                    {
                        //if (fFicheroO.Listline[i].iPosOrigen[k] == 15)
                        //{
                        //    List<char> cCharactertoReplace = new List<char> { '&', '*', '+', '-' };
                           
                        //    for (int l = 0; l < cCharactertoReplace.Count; l++)
                        //    {
                          
                        //        string cadena = cCharactertoReplace[l].ToString();
                        //        ListValuesLine[fFicheroO.Listline[i].iPosOrigen[k] - 1] = ListValuesLine[fFicheroO.Listline[i].iPosOrigen[k] - 1].Replace(cadena, " / ");
                        //    }
                        //}

                        if (k == iAux - 1)
                        {
                            ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] = ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] + ListValuesLine[fFicheroO.Listline[i].iPosOrigen[k] - 1];
                        } else
                        {
                            ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] = ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] + ListValuesLine[fFicheroO.Listline[i].iPosOrigen[k] - 1] + ". ";
                        }
                    }
                }
                else if (fFicheroO.Listline[i].sTypeMove == "IN")
                {
                    string scadenatoadd = ListValuesLine[fFicheroO.Listline[i].iPosOrigen[0] - 1];
                    List<char> lscadenatoadd = scadenatoadd.ToList();
                    List<char> lcAux = fFicheroO.Listline[i].sFinalValue.ToList();
                    int iPos = lcAux.FindIndex(n => n == '*');
                    string sNewCadena = string.Concat(lcAux.GetRange(0, iPos)) +  scadenatoadd +string.Concat(lcAux.GetRange(iPos + 1, lcAux.Count - iPos - 1)) ;
                    ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] = sNewCadena;
                }
                else if (fFicheroO.Listline[i].sTypeMove == "FILE")
                {
                    int iIndex_family = fFicheroO.Listline[i].lsfamilies.FindIndex(n=>n.Equals(ListValuesLine[fFicheroO.Listline[i].iPosOrigen[0] - 1]));
                    if (iIndex_family == -1)
                    {
                        ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] = "Familia no existe";
                    }
                    else
                    { 
                        ListFinalValuesLine[fFicheroO.Listline[i].iPosFinal - 1] = fFicheroO.Listline[i].lsidcategroias[iIndex_family];
                    }
                }
            }
        }

        private static void SimpleLineCopy()
        {

        }

        private static void OLD_ProcessItemforfinalFile(int iContItemInALine, ref string sItem, ref string lineFinal)
        {
            switch (iContItemInALine)
            {
                case 5: //Código que pasa a ser referencia
                    {
                        sItem = "017" + sItem + ",";
                        lineFinal = lineFinal + string.Concat(sItem);
                        break;
                    }
                case 6: //Titulo que pasa a ser Nombre
                    {
                        sNombre = sItem + ". ";
                        break;
                    }
                case 8:
                    {
                        
                        break;
                    }
                case 9:
                    {
                        
                        break;
                    }
                case 14: //Editorial que pasa a ser Nombre
                    {
                        sAux = sItem + ". ";
                        break;
                    }
                case 15: //Autores que pasa a ser Nombre
                    {
                        sNombre = sNombre + sItem + ". " + sAux;
                        break;
                    }
                case 16: //Instrumentación  que pasa a ser Nombre
                    {
                        sNombre = sNombre + sItem;
                        sItem = sNombre;
                        lineFinal = lineFinal + string.Concat(sItem);
                        break;
                    }
                default:
                    break;
                    

            }
        }
    }
}
