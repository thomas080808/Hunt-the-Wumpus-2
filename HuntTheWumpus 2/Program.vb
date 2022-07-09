Imports System

Module Program
    Const NumeroStanze As Integer = 32

    '0 = stanza normale senza niente
    '1 = stanza di sangue
    '2 = stanza di muschio
    '3 = stanza del mostro
    '10 = stanza con una freccia
    Enum TipoDiStanza


        Normale = 0
        Sangue
        Muschio
        Wumpus
        Freccia = 10
    End Enum

    Const NumeroDiStanzeMuschio As Integer = 6
    Const NumeroDistanzeSangue As Integer = 9
    'const NumeroDiStanzeWumpus As Integer = 1
    Const NumeroDiStanzeFrecce As Integer = 5


    Sub Main(args As String())


        Dim stanze(NumeroStanze) As Integer ' Stanze del gioco
        '() sono un vettore
        ' 11/10 = 1 resto 1
        ' 12/10 = 1 resto 2

        VBMath.Randomize() 'subroutine che genera un numero casuale poi chiamare "Rnd" è una funzione random    

        Dim StanzaMostro As Integer = (Rnd(Now.Millisecond()) * 100) Mod (NumeroStanze - 8) + 7

        Debug.Print("Numero stanza del mostro : {0}", StanzaMostro)

        'max
        'mostro | sangue max
        '        |
        '  23    |    25
        '  22    |    24
        '  21    |    23
        '  25    |    25


        Dim StanzaMassima As Integer = (NumeroStanze - StanzaMostro)

        If (StanzaMassima > (NumeroDistanzeSangue / 2)) Then

            StanzaMassima = (NumeroDistanzeSangue / 2)


        End If

        Dim stanzaMinima As Integer = (StanzaMostro + StanzaMassima) - NumeroDistanzeSangue
        Debug.Print("{0} {1}", stanzaMinima, StanzaMostro, StanzaMassima)
        'per fare la parentesi graffa bisogna premere: il tasto ALT GR + MAIUSCOLO + I DUE TASTI DELLE PARENTESI QUADRE
        ' for va da un numero ad un altro incrementandolo di uno ogni volta fino alla fine
        For a As Integer = stanzaMinima To StanzaMostro + StanzaMassima

            stanze(a) = TipoDiStanza.Sangue

        Next

        stanze(StanzaMostro) = TipoDiStanza.Wumpus ' sovrascrive la stanza dove c'è il sangue

        Dim contatoreMuschio As Integer = NumeroDiStanzeMuschio

        While (contatoreMuschio > 0)

            Dim indiceRnd As Integer = (Rnd(Now.Millisecond()) * 100) Mod (NumeroStanze - 1)
            'Debug.Print(indiceRnd) ' utilizzato per test solo se crasha il programma

            If (stanze(indiceRnd) = TipoDiStanza.Normale) Then

                stanze(indiceRnd) = TipoDiStanza.Muschio
                contatoreMuschio = contatoreMuschio - 1

            End If

        End While


        Dim contatoreFrecce As Integer = NumeroDiStanzeFrecce

        While (contatoreFrecce > 0)

            Dim indiceRnd As Integer = (Rnd(Now.Millisecond()) * 100) Mod (NumeroStanze - 1)

            If (stanze(indiceRnd) = TipoDiStanza.Normale) Then


                stanze(indiceRnd) = TipoDiStanza.Freccia

                contatoreFrecce = contatoreFrecce - 1

            End If

        End While

        'solo per test (debug)

        For indice As Integer = 0 To stanze.Length() - 1

            Debug.Print("stanza n° {0} valore {1}", indice, stanze(indice))

        Next

        Dim contatoreFrecceGiocatore As Integer = 0
        Dim posizioneGiocatore As Integer = 1
        Dim wumpusMorto As Boolean = False
        Dim giocatoreMorto As Boolean = False

        Console.WriteLine("*******************************")
        Console.WriteLine("*Caccia il Wumpus versione 2*")
        Console.WriteLine("*******************************")

        While (wumpusMorto = False Or giocatoreMorto = False)

            Console.WriteLine("Ti trovi nella stanza numero {0}", posizioneGiocatore)
            Console.WriteLine("Scegli una stanza da [1 - {0}] : ", NumeroStanze)

            Dim stanzaScelta As Integer = Integer.Parse(Console.ReadLine())
            'Debug.Print(stanzaScelta)

            If (stanzaScelta < 1 Or stanzaScelta > NumeroStanze) Then


                Console.WriteLine("Errore stanza, sceglierne un'altra")

            Else

                posizioneGiocatore = stanzaScelta

                If (stanze(posizioneGiocatore) = TipoDiStanza.Freccia) Then

                    contatoreFrecceGiocatore = contatoreFrecceGiocatore + 1

                    stanze(posizioneGiocatore) = TipoDiStanza.Normale

                    Console.WriteLine("Hai preso una freccia")
                    Console.WriteLine("Totale frecce {0}", contatoreFrecceGiocatore)

                End If

                If (stanze(posizioneGiocatore) = TipoDiStanza.Muschio) Then

                    Console.WriteLine("C'è del muschio in questa stanza")
                    Console.WriteLine("Se vuoi salvarti devi rispondere a questa domanda 99+1")

                    Dim scelta As Integer = Integer.Parse(Console.ReadLine())

                    If (scelta <> 100) Then

                        Console.WriteLine("Sei morto, sei caduto in un pozzo profondo")
                        giocatoreMorto = True
                        Exit While

                    End If



                End If

                If (stanze(posizioneGiocatore) = TipoDiStanza.Sangue) Then

                    Console.WriteLine("In questa stanza c'è del sangue!!!")

                    Console.WriteLine("Vuoi mouverti oppure vuoi scoccare una freccia?")
                    Console.WriteLine("scrivere H per scoccare una freccia")

                    Dim scelta As String = Console.ReadLine()

                    If (scelta = "H" Or scelta = "h") Then

                        If (contatoreFrecceGiocatore = 0) Then

                            Console.WriteLine("Non hai frecce, devi andare avanti")

                        Else

                            Console.WriteLine("Dove vuoi scoccare la freccia? S per sinistra o D per destra")
                            Dim sceltaFreccia As String = Console.ReadLine()

                            If (sceltaFreccia = "S" Or sceltaFreccia = "s") Then

                                If ((posizioneGiocatore - StanzaMostro) <= 2) Then

                                    wumpusMorto = True
                                    Exit While

                                End If

                            End If

                            If (sceltaFreccia = "D" Or sceltaFreccia = "d") Then

                                If ((StanzaMostro - posizioneGiocatore) <= 2) Then

                                    wumpusMorto = True
                                    Exit While

                                End If

                            End If

                        End If

                    End If

                End If

                If (stanze(posizioneGiocatore) = TipoDiStanza.Wumpus) Then

                    Console.WriteLine("Sei morto il Wumpus ti ha mangiato vivo")

                    giocatoreMorto = True
                    Exit While

                End If

            End If

        End While
        If (wumpusMorto) Then

            Console.WriteLine("Win!!!")

        End If

        If (giocatoreMorto) Then

            Console.WriteLine("Game over...")

        End If

        'Dim memoria1(1000) As Integer ' memoria creata in stack
        'Dim memoria2() As Integer    ' memoria create in heap

        ' programma è suddiviso in due
        ' lo stack di memoria :
        ' crea una pila di dati, ma questa pila non può essere ridimensionata in esecuzione
        ' l'heap di memoria :
        ' crea una pila di dati, ma questa pila può essesre ridimensionata in esecuzione


        'memoria1(0) = 1
        'memoria1(1) = 3
        'memoria1(2) = 5
        ' ...


        'For indice As Integer = 0 To memoria1.Length() - 1

        '    memoria1(indice) = indice + 1

        '    memoria1(indice) = Rnd()


        '    memoria(0) = 1
        '    memoria(1) = 2
        '    memoria(999) = 1000

        'Next



        'ReDim memoria2(100)
        'memoria2(0) = 1

        ''ReDim memoria2(200) ' resetta la memoria2
        'ReDim Preserve memoria2(200) ' non resetta la memoria2

        ''call f()
        ''Dim x As Integer = tizio() ' non produce un valore (quindi è errato)

        'Dim valore0 As Integer = 100

        'tizio(valore0)

        'Debug.Print(valore0)

        ' usare le parentesi graffe per passare i valori al debuggger
        'Debug.Print("{0} {1}", valore0, valore1)

        'qui sotto non conosco i valori all'interno di tizio()

        'call g()
        'caio()
        'Dim x As Integer = caio()

        'Debug.Print(x)

        'qui sotto conosco il valore all'interno di caio()

    End Sub

    'subroutine (sub) accetta una lista di parametri ma non ritorna nessun valore
    'la subroutine può essere nominata come si vuole in lettere alfanumeriche (A...Z a...z 0...9)
    'Attenzione a non mettere numeri nella prima posizione nome
    'la lista dei parametri può essere passata per :
    ' - valore = Byval (copia il contenuto)
    ' - referenza = Byref (non copia il contenuto ma ne fa un'alias)
    'Sub tizio(ByVal y As Integer, ByRef z As Integer)
    'Sub tizio(ByRef x As Integer)

    '    Console.WriteLine("subroutine tizio")

    '    Debug.Print("subroutine tizio")

    '    x = x + 10

    'End Sub

    'funzione (function) accetta una lista di parametri e ritorna un valore
    'la funzione può essere nominata come si vuole in lettere alfanumeriche (A...Z a...z 0...9)
    'Attenzione a non mettere numeri nella prima posizione del nome
    'la lista dei parametri può essere passata per :
    ' - valore = Byval (copia il contenuto)
    ' - referenza = Byref (non copia il contenuto ma ne fa un'alias)
    'Sub tizio(ByVal y As Integer, ByRef z As Integer)
    'Function caio() As Integer

    '    Console.WriteLine("funzione caio")

    '    Debug.Print("funzione caio")

    '    Dim x As Integer = 100

    '    x = x + 10

    '    Return x

    'End Function

    'quando utilizzare la funzione invece di una subroutine?
    'dipende se vuoi un valore di ritorno dalla funzione chiamata 
    'sub e function fanno le stesse cose all'interno solo che la funzione ritorna un valore che può servire per altri scopi

End Module