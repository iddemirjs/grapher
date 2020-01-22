using System;
using System.IO;

namespace Grapher
{
    class Program
    {
        public static char[,] map = new char[25, 40];
        public static int[,] RMatrix = new int[16, 16];
        public static string validLettersTemplate = "ABCDEFGHIJKLMNOP";
        static string[] letterCordinates = new string[16];
        public static int
            MapStartingPointX = 4,
            MapStartingPointY = 4,
            RMatrixPositionX = MapStartingPointX + map.GetLength(1) + 10,
            RMatrixPositionY = MapStartingPointY,
            RnMatrixPositionX = MapStartingPointX + map.GetLength(1) + 10,
            RnMatrixPositionY = MapStartingPointY + RMatrix.GetLength(1) + 4,
            MinStepPositionFromX = 31,
            MinStepPositionToX = 37,
            MinStepTextPositionY = MapStartingPointY + map.GetLength(0) + 3,
            MinStepTextPositionX = MapStartingPointX,
            AnswerTextPositionX = MapStartingPointX,
            AnswerPositionX = MapStartingPointX + 8,
            AnswerPositionY = MinStepTextPositionY + 2,
            RnTextTitlePositionY = RnMatrixPositionY - 2;


        public static ConsoleColor borderColor = ConsoleColor.Magenta,
                                   borderNumberColor = ConsoleColor.DarkRed,
                                   inCanvasColor = ConsoleColor.Cyan,
                                   titleRMatrixColor = ConsoleColor.Magenta,
                                   titleRnMatrixColor = ConsoleColor.Green,
                                   minStepInputTextColor = ConsoleColor.DarkGreen,
                                   minStepAnswerTextColor = ConsoleColor.Red,
                                   menuTextColor = ConsoleColor.Red,
                                   newWritingColor= ConsoleColor.DarkRed;

        static void Main(string[] args)
        {
            while (true)
            {
                int result = writeMenu();
                if (result == 1)
                {
                    for (int y = 0; y < map.GetLength(0); y++)
                    {
                        for (int x = 0; x < map.GetLength(1); x++)
                        {
                            map[y, x] = '.';
                        }
                    }
                    writeGrapherScreen();
                    listenKeyboard();
                }
                else if (result == 2)
                {
                    writeGrapherScreen();
                    string readMaptResult = readMap();
                    if (readMaptResult != "OK")
                    {
                        Console.Clear();
                        Console.WriteLine("Fatal Error: " + readMaptResult + "! Please key some button for continue.");
                        Console.ReadKey();
                        continue;
                    }
                    PrintMapToScreen(map, MapStartingPointX, MapStartingPointY);
                    listenKeyboard();
                }
                else if (result == 3)
                {
                    int wInt, hInt;
                    while (true) {
                        Console.Write("Please enter your canvas height( " + map.GetLength(0) + " ) : ");
                        string h = Console.ReadLine();
                        Console.Write("Please enter your canvas width( " + map.GetLength(1) + " ) : ");
                        string w = Console.ReadLine();
                        if (!int.TryParse(w, out wInt)) continue;
                        if (!int.TryParse(h, out hInt)) continue;
                        map = new char[hInt, wInt];
                        constantUpdater();
                        break;
                    }
                }
                else if (result == 4)
                {
                    System.Environment.Exit(1);
                }
            }

        }

        static void PrintMapToScreen(char[,] matrix, int matrixStartX, int matrixStartY)
        {
            Console.ForegroundColor = inCanvasColor;
            Console.SetCursorPosition(matrixStartX, matrixStartY);
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    Console.Write(matrix[x, y]);
                }
                Console.SetCursorPosition(MapStartingPointX, Console.CursorTop + 1);
            }
        }

        static void PrintRMatrixToScreen(int[,] matrix, int matrixStartX, int matrixStartY)
        {
            int tempSize = 0;
            for (int i = 0; i < letterCordinates.Length; i++)
            {
                if (letterCordinates[i] == null)
                {
                    tempSize = i;
                    break;
                }
            }

            Console.ForegroundColor = inCanvasColor;
            Console.SetCursorPosition(matrixStartX, matrixStartY);
            for (int i = 0; i < tempSize; i++)
            {
                Console.Write(validLettersTemplate[i] + " ");
            }



            for (int x = 0; x < tempSize; x++)
            {
                Console.SetCursorPosition(matrixStartX - 2, Console.CursorTop + 1);
                Console.Write(validLettersTemplate[x] + " ");
                for (int y = 0; y < tempSize; y++)
                {
                    Console.Write(matrix[x, y] + " ");
                }

            }
        }

        public static string readMap()
        {
            StreamReader sr = new StreamReader("graph.txt");
            string line = " ";
            int index = 0;
            do
            {
                line = sr.ReadLine();
                if (line.Length == map.GetLength(1))
                {
                    for (int i = 0; i < map.GetLength(1); i++)
                    {
                        map[index, i] = line[i];
                    }
                    index++;
                }
                else
                {
                    return "Your txt file must have " + map.GetLength(1) + " horizantal character. But you have " + line.Length + ". ";
                }
            } while (!sr.EndOfStream);
            if (index == map.GetLength(0))
            {
                return "OK";
            }
            else
            {
                return "Your txt file must have " + map.GetLength(0) + " vertical character";
            }
            sr.Close();
        }

        // Kullanıcı Menuden bir karar aldığında ekranı temizleyip templateyi yazacak 
        public static void writeGrapherScreen()
        {
            Console.SetCursorPosition(MapStartingPointX, MapStartingPointY - 2);
            Console.ForegroundColor = borderNumberColor;
            for (int j = 0; j < map.GetLength(1); j++)
                Console.Write(j % 10);

            Console.ForegroundColor = borderColor;
            Console.SetCursorPosition(MapStartingPointX, MapStartingPointY - 1);
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Console.Write("#");
            }
            Console.SetCursorPosition(MapStartingPointX - 2, MapStartingPointY);
            for (int j = 0; j < map.GetLength(0); j++)
            {
                Console.CursorLeft = MapStartingPointX - 2;
                Console.ForegroundColor = borderNumberColor;
                Console.Write(j % 10);
                Console.ForegroundColor = borderColor;
                Console.SetCursorPosition(MapStartingPointX - 1, MapStartingPointY + j);
                Console.Write('#');
                Console.ForegroundColor = inCanvasColor;
                for (int q = 0; q < map.GetLength(1); q++)
                {
                    Console.Write(".");
                }
                Console.ForegroundColor = borderColor;
                Console.WriteLine('#');
            }
            Console.ForegroundColor = borderColor;
            Console.CursorLeft = MapStartingPointX;
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Console.Write("#");
            }
            Console.SetCursorPosition(RMatrixPositionX, RMatrixPositionY - 2);
            Console.ForegroundColor = titleRMatrixColor;
            Console.Write("R Matrix");
            Console.SetCursorPosition(RMatrixPositionX, RMatrixPositionY - 1);
            Console.Write("------------------");
            Console.SetCursorPosition(MapStartingPointX, MapStartingPointY);
            Console.SetCursorPosition(RnMatrixPositionX, RnMatrixPositionY - 2);
            Console.ForegroundColor = titleRnMatrixColor;
            Console.Write("R* Matrix");
            Console.SetCursorPosition(RnMatrixPositionX, RnMatrixPositionY - 1);
            Console.Write("------------------");

            Console.ForegroundColor = minStepInputTextColor;
            Console.SetCursorPosition(MinStepTextPositionX, MinStepTextPositionY);
            Console.Write("Query for min steps: from:   to:  ");

            Console.ForegroundColor = minStepAnswerTextColor;
            Console.SetCursorPosition(AnswerTextPositionX, AnswerPositionY);
            Console.Write("Answer: ");

        }

        // + pathini takip ederken yeni konumda nokta veya # bulduğunda yeni dogrultuyu tespit ederek
        // yeni doğrultunun yönünü belirtecek (Yeni doğrultuyu tespit ederken en yakın x veya + aranacak.)
        public static bool directionChangeControl(int xPosition, int yPosition) { return true; }

        // kullanıcı yeni doğrultu seçtiginde yeni doğrultunun uygununluğunu bu fonksiyona parametre girerek
        // bu fonksiyondan alınan cevaba göre kabul edilebilirliği belirlenecek.
        public static int[] newRotation(int xPosition, int yPosition) { return new int[2]; }

        // Kullanıcı graph çizeçeği zaman klavye dinleyecek
        public static void listenKeyboard()
        {
            ConsoleKey key;
            int cursorX = 15, cursorY = 15;
            Console.SetCursorPosition(cursorX, cursorY);
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true).Key;
                    while (Console.KeyAvailable) Console.ReadKey(true);
                    if (key == ConsoleKey.RightArrow && cursorX >= MapStartingPointX && cursorX < MapStartingPointX + map.GetLength(1) - 1)
                    {
                        cursorX++;
                        Console.SetCursorPosition(cursorX, cursorY);
                    }
                    else if (key == ConsoleKey.LeftArrow && cursorX > MapStartingPointX)
                    {
                        cursorX--;
                        Console.SetCursorPosition(cursorX, cursorY);
                    }
                    else if (key == ConsoleKey.UpArrow && cursorY > MapStartingPointY)
                    {
                        cursorY--;
                        Console.SetCursorPosition(cursorX, cursorY);
                    }
                    else if (key == ConsoleKey.DownArrow && cursorY < MapStartingPointY + map.GetLength(0) - 1)
                    {
                        cursorY++;
                        Console.SetCursorPosition(cursorX, cursorY);
                    }
                    else if ((Convert.ToInt16(key) >= 65 && Convert.ToInt16(key) <= 80))
                    {
                        Console.ForegroundColor = newWritingColor;
                        Console.SetCursorPosition(cursorX, cursorY);
                        Console.Write(key.ToString().ToUpper());
                        Console.SetCursorPosition(cursorX, cursorY);
                        map[cursorY - MapStartingPointY, cursorX - MapStartingPointX] = Convert.ToChar(key.ToString().ToUpper());
                    }
                    else if (key == ConsoleKey.Spacebar)
                    {
                        Console.ForegroundColor = newWritingColor;
                        Console.SetCursorPosition(cursorX, cursorY);
                        Console.Write("+");
                        Console.SetCursorPosition(cursorX, cursorY);
                        map[cursorY - MapStartingPointY, cursorX - MapStartingPointX] = '+';
                    }
                    else if (key == ConsoleKey.X)
                    {
                        Console.ForegroundColor = newWritingColor;
                        Console.SetCursorPosition(cursorX, cursorY);
                        Console.Write("X");
                        Console.SetCursorPosition(cursorX, cursorY);
                        map[cursorY - MapStartingPointY, cursorX - MapStartingPointX] = 'X';
                    }
                    else if (Convert.ToInt16(key) == 190)
                    {
                        Console.ForegroundColor = newWritingColor;
                        Console.SetCursorPosition(cursorX, cursorY);
                        Console.Write(".");
                        Console.SetCursorPosition(cursorX, cursorY);
                        map[cursorY - MapStartingPointY, cursorX - MapStartingPointX] = '.';
                    }
                    else if (key == ConsoleKey.Q)
                    {
                        while (true)
                        {
                            Console.SetCursorPosition(MinStepPositionFromX, MinStepTextPositionY);
                            char fromLetter = Console.ReadKey().KeyChar;
                            fromLetter = Convert.ToChar(fromLetter.ToString().ToUpper());
                            Console.SetCursorPosition(MinStepPositionToX, MinStepTextPositionY);
                            char toLetter = Console.ReadKey().KeyChar;
                            toLetter = Convert.ToChar(toLetter.ToString().ToUpper());
                            if (validLettersTemplate.IndexOf(fromLetter) >= 0 && validLettersTemplate.IndexOf(toLetter) >= 0)
                            {
                                int graphResult = startGraphTrace();
                                if (graphResult == 1)
                                {
                                    int minStepResult = getMinStepNumber(fromLetter, toLetter);
                                    Console.SetCursorPosition(AnswerPositionX, AnswerPositionY);
                                    string answerText = (minStepResult == -1) ? "Not possible" : minStepResult.ToString();
                                    Console.Write(answerText+"                                       ");
                                    Console.SetCursorPosition(AnswerPositionX, AnswerPositionY);
                                    break;
                                }
                                else
                                {
                                    Console.SetCursorPosition(AnswerPositionX, AnswerPositionY);
                                    Console.Write("Your graph is not valid.");
                                    break;
                                }
                            }
                            else
                            {
                                Console.SetCursorPosition(MinStepPositionFromX, MinStepTextPositionY);
                                Console.Write(' ');
                                Console.SetCursorPosition(MinStepPositionToX, MinStepTextPositionY);
                                Console.Write(' ');
                                Console.SetCursorPosition(AnswerPositionX, AnswerPositionY);
                                Console.Write("Please Enter acceptable letter.");
                            }
                        }

                    }
                    else if (Convert.ToInt16(key) >= 48 && Convert.ToInt16(key) <= 57)
                    {
                        int powerOfMatrix = Convert.ToInt16(key) - 48;
                        int graphResult = startGraphTrace();
                        if (graphResult == 1)
                        {
                            if (powerOfMatrix == 0)
                            {
                                Console.ForegroundColor = titleRnMatrixColor;
                                Console.SetCursorPosition(RnMatrixPositionX, RnTextTitlePositionY);
                                Console.Write("Rmin Matrix");
                                Console.ForegroundColor = inCanvasColor;
                                int[,] Rmin = RminStepMatrix(RMatrix);
                                PrintRMatrixToScreen(Rmin, RnMatrixPositionX + 1, RnMatrixPositionY);
                            }
                            else
                            {
                                string outputText = powerOfMatrix.ToString();
                                if (powerOfMatrix == 1) outputText = "*";
                                Console.ForegroundColor = titleRnMatrixColor;
                                Console.SetCursorPosition(RnMatrixPositionX, RnTextTitlePositionY);
                                Console.Write("R" + outputText + " Matrix");
                                Console.ForegroundColor = inCanvasColor;
                                PrintRMatrixToScreen(RMatrix, RMatrixPositionX + 1, RMatrixPositionY);
                                int[,] powerOfRnMatrix = (powerOfMatrix == 1) ? RStarFinder(RMatrix) : CalculatingRnMatrix(RMatrix, powerOfMatrix);
                                PrintRMatrixToScreen(powerOfRnMatrix, RnMatrixPositionX, RnMatrixPositionY);
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(AnswerPositionX, AnswerPositionY);
                            Console.Write("Your graph is not valid.");
                        }
                    }
                    else if (key == ConsoleKey.S)
                    {
                        int graphResult = startGraphTrace();
                        if (graphResult == 1)
                        {
                            exportGraph();
                            Console.SetCursorPosition(AnswerPositionX, AnswerPositionY);
                            Console.Write("Your graph is exported.");

                        }
                        else
                        {
                            Console.SetCursorPosition(AnswerPositionX, AnswerPositionY);
                            Console.Write("Your graph is not valid.");
                            
                        }
                    }

                }
            }
        }

        public static int getMinStepNumber(char fromLetter, char toLetter)
        {
            int i = 0;
            for (i = 0; i < letterCordinates.Length; i++)
            {
                if (letterCordinates[i] == null)
                {
                    i--;
                    break;
                }
            }
            if (validLettersTemplate.IndexOf(fromLetter) <= i && validLettersTemplate.IndexOf(toLetter) <= i)
            {
                int minStepNumber = RminStepFinder(fromLetter, toLetter, RMatrix);
                return minStepNumber;
            }
            else
            {
                return -2;
            }
        }

        #region Tracing Graph Functions
        // Kullanıcı graph çizimini tamamladığında veya yeni graphi dosyadan okuttugunda
        // yeni graphın uygunluğunu, R  ve R* komsuluk matrisi cıkaran fonksıyon. 
        // Bu fonksiyon OK dondururse harfleri basarı ıle kontrol etmıstır.
        public static string detectGraphLetters()
        {
            letterCordinates = new string[16];
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] != '.' && map[y, x] != 'X' && map[y, x] != '+')
                    {
                        int indexLetter = validLettersTemplate.IndexOf(Convert.ToString(map[y, x]).ToUpper());
                        if (indexLetter != -1)
                        {
                            if (letterCordinates[indexLetter] == null)
                            {
                                letterCordinates[indexLetter] = x.ToString() + "," + y.ToString();
                            }
                            else
                            {
                                return "You can't use same letter one more time";
                            }
                        }
                        else
                        {
                            return "You can't use invalid letters.";
                        }
                    }
                }
            }

            int id = 0;
            int didSawSpace = 0;
            bool isValidLettersOrder = true;
            foreach (var item in letterCordinates)
            {

                if (item == null)
                {
                    didSawSpace = 1;
                }
                else if (item != null && didSawSpace == 1)
                {
                    isValidLettersOrder = false;
                    break;
                }
                //Console.SetCursorPosition(0, 60);
                //Console.Write(validLettersTemplate[id]);   
                //Console.WriteLine(item);
                id++;
            }
            if (!isValidLettersOrder)
            {
                return "You should use ordering letters.";
            }
            else
            {
                return "OK";
            }

        }

        // Harfin etrafında dönerek + ları tespit edecek.
        public static string pivotOnLetter(int letterX, int letterY, char letter)
        {
            for (int dx = letterX - 1; dx <= letterX + 1; dx++)
            {
                for (int dy = letterY - 1; dy <= letterY + 1; dy++)
                {
                    if (letterX == dx && letterY == dy) continue;
                    if (inCanvasLimit(letterX, letterY, map) && (map[dy, dx] == '+'))
                    {
                        char targetVertex = findPlusEnd(dx, dy, dx - letterX, dy - letterY);
                        if (targetVertex == 'U') return letter + " letter has something wrong.";
                        Console.SetCursorPosition(MapStartingPointX + dx, MapStartingPointY + dy);
                        int startLetterIndis = validLettersTemplate.IndexOf(letter);
                        int finishLetterIndis = validLettersTemplate.IndexOf(targetVertex);
                        RMatrix[startLetterIndis, finishLetterIndis] = 1;
                    }
                }
            }
            return "OK";
        }
        /* 
         * + Noktasından başlıyarak yonu bellı bır noktaya kadar takıp eden function
         * bu fonksiyon harf bulana kadar calısır bulamassa graph yanlıştir - dondurur
         */
        public static char findPlusEnd(int x, int y, int directionX, int directionY)
        {
            while (true)
            {
                if (inCanvasLimit(x + directionX, y + directionY, map) && map[y + directionY, x + directionX] == '+')
                {
                    x += directionX;
                    y += directionY;
                }
                else if (inCanvasLimit(x + directionX, y + directionY, map) && map[y + directionY, x + directionX] == 'X')
                {
                    return map[y + directionY * 2, x + directionX * 2];
                }
                else if (inCanvasLimit(x + directionX, y + directionY, map) || map[y + directionY, x + directionX] != '+')
                {
                    string newRoutesStr = findNewDirection(x, y, directionX, directionY);
                    if (newRoutesStr != "-1")
                    {
                        string[] newRoutes = newRoutesStr.Split(',');
                        directionX = Convert.ToInt16(newRoutes[0]);
                        directionY = Convert.ToInt16(newRoutes[1]);
                    }
                    else
                    {
                        return 'U';
                    }

                }

            }
        }
        /*
         * Bu fonksiyon kontrol edilmek istenen x ve y kordinatının  
         * map arrayının sınırları icinde mi oldugunu kontrol eder.
         * değilse false sınırlar içindeyse true dondürür.
        */
        public static bool inCanvasLimit(int x, int y, char[,] myArray)
        {
            if ((x >= 0 && x <= myArray.GetLength(1)) && (y >= 0 && y <= myArray.GetLength(0)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /* 
         * Bu fonksiyon bir artının rotası değiştiğinde yeni rotasını belirler
         * ve bu yeni rotayı string x,y şeklinde dönrürür. döndürülen bu string
         * yerinde parçalanarak  x y kordinat dizisine dönüştürülür.
         * Eğer bu fonksiyon string -1 döndürürse rotasyon çiziminde hata olduğunu belirtir.
         */
        public static string findNewDirection(int plusX, int plusY, int directionX, int directionY)
        {
            string newResult = null;
            for (int newX = plusX - 1; newX <= plusX + 1; newX++)
            {
                for (int newY = plusY - 1; newY <= plusY + 1; newY++)
                {
                    //Console.SetCursorPosition(MapStartingPointX+newX,MapStartingPointY+newY);
                    //Console.ForegroundColor = ConsoleColor.Green;
                    //Console.Write(map[newY,newX]);
                    if (newX == plusX && newY == plusY) continue;
                    //if (newX - plusX == directionX && newY - plusY == directionY) continue;
                    if (plusX - directionX == newX && plusY - directionY == newY) continue;
                    if ((map[newY, newX] == '+' || map[newY, newX] == 'X') && newResult != null) return "-1";
                    if ((map[newY, newX] == '+' || map[newY, newX] == 'X') && newResult == null) newResult = newResult = (newX - plusX).ToString() + ',' + (newY - plusY).ToString();
                }
            }
            return newResult;
        }
        #endregion

        public static int writeMenu()
        {
            Console.Clear();
            Console.ForegroundColor = menuTextColor;
            Console.WriteLine("PLEASE SELECT FROM THE OPTIONS BELOW: ");
            Console.WriteLine("1-DRAW A GRAPH\n2-LOAD A GRAPH\n3-SETTINGS\n4-EXIT");
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice >= 1 && choice <= 4)
                    {
                        Console.Clear();
                        return choice;
                    }
                    else
                    {
                        Console.WriteLine("Please Enter between 1 and 2");
                    }
                }
                else
                {
                    Console.WriteLine("Your choice must be integer.");
                }
            }

        }

        static int[,] ProductTwoMatrix(int[,] a, int[,] b)
        {
            int[,] result = new int[a.GetLength(0), b.GetLength(1)];
            int sum = 0;
            for (int i = 0; i < a.GetLength(1); i++)
            {
                for (int j = 0; j < b.GetLength(0); j++)
                {
                    sum = 0;
                    for (int k = 0; k < a.GetLength(0); k++)
                        sum = sum + a[i, k] * b[k, j];
                    if (sum >= 1)
                        sum = 1;
                    result[i, j] = sum;
                }
            }
            return result;
        }

        static int[,] CalculatingRnMatrix(int[,] a, int powerDegree)
        {
            int[,] matrixOld = new int[a.GetLength(0), a.GetLength(1)];
            int[,] RnResult = new int[a.GetLength(0), a.GetLength(1)];
            if (powerDegree == 1) return a;
            for (int y = 0; y < a.GetLength(0); y++)
            {
                for (int x = 0; x < a.GetLength(1); x++)
                {
                    matrixOld[y, x] = a[y, x];
                }
            }

            for (int power = 1; power < powerDegree; power++)
            {
                int sum = 0;
                for (int i = 0; i < matrixOld.GetLength(1); i++)
                {
                    for (int j = 0; j < a.GetLength(0); j++)
                    {
                        sum = 0;
                        for (int k = 0; k < matrixOld.GetLength(0); k++)
                            sum = sum + matrixOld[i, k] * a[k, j];
                        if (sum >= 1)
                            sum = 1;

                        RnResult[i, j] = sum;

                    }
                }
                for (int y = 0; y < RnResult.GetLength(0); y++)
                {
                    for (int x = 0; x < RnResult.GetLength(1); x++)
                    {
                        matrixOld[y, x] = RnResult[y, x];
                    }
                }
            }
            return RnResult;
        }

        static int[,] RStarFinder(int[,] RMatrix)
        {
            int[,] RStar = new int[RMatrix.GetLength(0), RMatrix.GetLength(1)];
            for (int y = 0; y < RMatrix.GetLength(0); y++)
            {
                for (int x = 0; x < RMatrix.GetLength(1); x++)
                {
                    RStar[y, x] = RMatrix[y, x];
                }
            }
            for (int power = 2; power <= RMatrix.GetLength(0); power++)
            {
                int[,] RnMatrix = CalculatingRnMatrix(RMatrix, power);
                for (int y = 0; y < RnMatrix.GetLength(0); y++)
                {
                    for (int x = 0; x < RnMatrix.GetLength(1); x++)
                    {
                        RStar[y, x] += RnMatrix[y, x];
                    }
                }
            }
            for (int y = 0; y < RStar.GetLength(0); y++)
            {
                for (int x = 0; x < RStar.GetLength(1); x++)
                {
                    if (RStar[y, x] >= 1)
                    {
                        RStar[y, x] = 1;
                    }
                    else if (RStar[y, x] <= 1)
                    {
                        RStar[y, x] = 0;
                    }
                }
            }
            return RStar;
        }

        static int RminStepFinder(char startPoint, char endPoint, int[,] RMatrix)
        {
            int[,] RnMatrix = new int[RMatrix.GetLength(0), RMatrix.GetLength(1)];
            int startIndisY = validLettersTemplate.IndexOf(startPoint);
            int endIndisX = validLettersTemplate.IndexOf(endPoint);
            for (int power = 1; power <= RMatrix.GetLength(0); power++)
            {
                RnMatrix = CalculatingRnMatrix(RMatrix, power);
                if (RnMatrix[startIndisY, endIndisX] == 1)
                {
                    return power;
                }
            }

            return -1;
        }

        static int[,] RminStepMatrix(int[,] RMatrix)
        {
            int[,] RminMatrix = new int[16, 16];
            for (int y = 0; y < RminMatrix.GetLength(0); y++)
            {
                for (int x = 0; x < RminMatrix.GetLength(1); x++)
                {
                    RminMatrix[y, x] = 0;
                }
            }
            for (int p = 1; p < RminMatrix.GetLength(0); p++)
            {
                int[,] tempMatrix = CalculatingRnMatrix(RMatrix, p);
                for (int y = 0; y < tempMatrix.GetLength(0); y++)
                {
                    for (int x = 0; x < tempMatrix.GetLength(1); x++)
                    {
                        if (tempMatrix[y, x] == 1 && RminMatrix[y, x] == 0)
                        {
                            RminMatrix[y, x] = p;
                        }
                    }
                }
            }
            return RminMatrix;
        }

        static int startGraphTrace()
        {
            string result = detectGraphLetters();
            if (result != "OK") /*answer alanına result yazdır.*/ return -1;
            for (int i = 0; i < letterCordinates.Length; i++)
            {
                if (letterCordinates[i] == null) break;
                string[] vertexCoordinate = letterCordinates[i].Split(',');
                string pivotResult = pivotOnLetter(Convert.ToInt16(vertexCoordinate[0]), Convert.ToInt16(vertexCoordinate[1]), validLettersTemplate[i]);
                if (pivotResult != "OK")/* Answera hatayı yaz */ return -1;
            }
            return 1;
        }

        static void colorChanger(int x = -1, int y = -1, ConsoleColor consoleColor=ConsoleColor.White)
        {
            if (x!=-1 && y!=-1)
            {
                Console.SetCursorPosition(x, y);
            }
            Console.ForegroundColor=consoleColor;
        }

        static void constantUpdater()
        {
            MapStartingPointX = 4;
            MapStartingPointY = 4;
            RMatrixPositionX = MapStartingPointX + map.GetLength(1) + 10;
            RMatrixPositionY = MapStartingPointY;
            RnMatrixPositionX = MapStartingPointX + map.GetLength(1) + 10;
            RnMatrixPositionY = MapStartingPointY + RMatrix.GetLength(1) + 4;
            MinStepPositionFromX = 31;
            MinStepPositionToX = 37;
            MinStepTextPositionY = MapStartingPointY + map.GetLength(0) + 3;
            MinStepTextPositionX = MapStartingPointX;
            AnswerTextPositionX = MapStartingPointX;
            AnswerPositionX = MapStartingPointX + 8;
            AnswerPositionY = MinStepTextPositionY + 2;
            RnTextTitlePositionY = RnMatrixPositionY - 2;
        }

        public static void exportGraph()
        {
            StreamWriter sr = File.CreateText("grapherOutput.txt");

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    sr.Write(map[y, x]);
                }
                sr.WriteLine();
            }

            sr.Close();
        }
    }
}