# Antipalindromic-numbers

This application is used to calculate palindromic and antipalindromic numbers in various bases and with a variety of different properties.


The application first needs to be installed using the executable file setup.exe. After the installation and a succesful launch, it presents its only window 
to the user. The combo box lets the user select one of the nine modes and lists a set of parameters needed to perform the computation. All calculations are launched 
by pressing the Start button.


In the 'Spaces between palindromic and antipalindromic numbers' mode, a desired base and a maximum value must be assigned. After that, all palindromic and 
antipalindromic numbers from the selected range are shown, along with their respective expansions, expansion lengths and, in the end, the maximum number of palindromic 
numbers between two antipalindromic numbers and vice-versa in the selected range. The 'Palindromic primes' mode is used to calculate all palindromic primes 
in the desired base and range and their expansions, and shows their number. The mode 'Antipalindromic primes in base 3' only needs to be given a maximum value 
and shows all antipalindromic numbers in base 3 less than the selected value, their expansions, and their number. (There is never more than one antipalindromic prime 
in any other base.) The 'Multi-base antipalindromic numbers' mode lets the user choose two bases and a maximum and presents all numbers from 1 to the maximum 
that are antipalindromic in both of these bases, and their expansion in each base. After ticking the check box 'Three bases', an additional base can be selected 
and the application shows only the numbers that are antipalindromic in all three bases at once. 

In the mode 'All bases in which a number is antipalindromic', the user inputs the desired number range and after the launch, the application lists all bases 
in which each number from the desired range has an antipalindromic expansion and, in the end, lists apart all numbers from the desired range that only have one such base. 
The following mode, 'Sums of 3 antipalindromic numbers in base 3', again offers the user the choice of a number range and shows one of the possible ways each number 
can be expressed as a sum of three antipalindromic numbers in base 3. If the number cannot be expressed as such, the application only shows the message 
'CANNOT BE A SUM OF THREE ANTIPALINDROMIC NUMBERS IN BASE 3' for this number. The mode 'Sums of 3 antipalindromic numbers in base 3 (only for palindromic integers)' is 
very similar. However, it only lists palindromic numbers from the selected range. The eighth mode, 'Squares and higher powers', needs to be given a maximum base, an exponent, 
and a maximum value. For each base, from 2 up to the selected one, the application lists all numbers from the selected range that are antipalindromic and have an integer 
root for the required exponent, along with their respective expansions and their number. The last mode, 'Squares and higher powers (only for bases n^k, n^k+1, n^k+2)', 
is used to show the difference between “n^k+1” bases and the rest, in terms of the number of antipalindromic k-th powers. These bases are likely to have many 
antipalindromic k-th powers. The user only inputs a desired exponent and a maximum value of the number. The optimal maximum base is, in contrast with the previous mode, 
calculated by the application.  

The user is allowed to tick the box 'File output', which opens up a dialogue window, letting the user create an output file in a desired location. 
The output is then both presented on the screen and written to the text file.
