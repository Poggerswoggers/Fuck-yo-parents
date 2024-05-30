->Ronaldo

VAR correctAnswer = ""
VAR questionName = ""
EXTERNAL LoadQuestion(name)
==Ronaldo==
~correctAnswer = 1
What do you want?
    *[Do you need any help?] 
        That’ll be lovely  
            ->loadMCQ
    *[(Starts laughing at them)]
        What's so funny? Don’t bother me! 
            ->loadMCQ
    *[How much does that stick cost?]
        Get out of my way.
            ->loadMCQ
    *[Bet I could outrun you.]
        Anyone can outrun me, move.
            ->loadMCQ

         

==Jaylen==
~correctAnswer = 0
I hate peak hours.
    *[I hate it too]
        Someone gets it.
            -> DONE
    *[Do you see anyone that needs help?] //Hints 
        There's a guy who looks GRUMPY maybe he need some help?
            -> DONE
            

==Naomi==
~correctAnswer = 0
It stinks around here.
    *[Somebody did not shower]
        Probably.
            -> DONE
    *[Do you see anyone that needs help?] //Hints
        I saw someone that has CRUTCHES, I think they need some help.
            -> DONE
            
            
            
            
            
            
            
            
            
==loadMCQ==
->DONE