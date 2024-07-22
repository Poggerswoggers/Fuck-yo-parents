VAR correctAnswer = ""


==Ronaldo==
~correctAnswer = 1
Excuse me.
    *[Do you need any help?] 
        That’ll be lovely  
            ->DONE
    *[(Starts laughing at them)]
        What's so funny? Don’t bother me! 
            ->DONE
    *[How much does that stick cost?]
        Get out of my way.
            ->DONE
    *[Bet I could outrun you.]
        Anyone can outrun me, move.
            ->DONE
==Gary==
~correctAnswer = 1
Hello.
    *[Let me help you.] 
         I can handle myself.
            ->DONE
    *[I’ll lead the way.]
        No need to help, I got it.
            ->DONE
    *[Would you like me to help you?]
        That’ll be nice. 
            ->DONE
    *[How many fingers am I holding up?]
        That’s not very nice.
            ->DONE

         

==Jaylen==
~correctAnswer = 0
I hate peak hours.
    *[I hate it too]
        Someone gets it.
            -> DONE
    *[Do you see anyone that needs help?] //Hints 
        I saw someone that has CRUTCHES, I think they need some help.
            -> DONE
    *[Whatever it takes to get home.] //Hints 
        Yeah, I’ll just have to squeeze in for a while.
            -> DONE
            

==Naomi==
~correctAnswer = 0
Do all pregnant ladies have big baby bumps?
    *[Yes]
        I don’t think so. You may not be able to tell if someone is pregnant as her baby bump may not be visible or she may be wearing loose clothing.
            -> DONE
    *[No] 
        That’s right! You may not be able to tell if someone is pregnant as her baby bump may not be visible or she may be wearing loose clothing.
            -> DONE
    *[Do you see anyone that needs help?] //Hints
        I saw someone that has CRUTCHES, I think they need some help.
            -> DONE
            
==Nicholas== 
~correctAnswer = 0
Would you rather have $1 or $2?            
    *[Is there someone that could use some help?]        
        There’s someone wearing SUNGLASSES, he might need some help? 
            -> DONE
    *[$1]        
        Wrong answer, next time we go big.
            -> DONE
    *[$2]
         That’s right. Go big!   
            -> DONE
    *[What are you saying?]
         Only the real ones know. 
            -> DONE
            
==Seth== 
~correctAnswer = 0
Yes?            
    *[You look slick,where did you get your clothes?]        
        From the thrift store, I like the vintage fit. 
            -> DONE
    *[Where are you headed to?]        
        I am heading to work. 
            -> DONE
    *[Is there someone that could use some help?]
         There’s someone that is walking with a GUIDE CANE, he might need some help?  
            -> DONE
            
==Garreth==  
~correctAnswer = 0
Why is being caring on public transport important?            
    *[So that we can assist commuters who needs help.]        
        That’s right! Together, we can be Caring Commuters!
            -> DONE
    *[So we can ignore others and be focused on ourselves only.]        
        I don’t think that is right, we should not ignore others who need help!
            -> DONE
    *[So we can use our phones.]
        I don’t think that is right, we should not ignore others who need help!  
            -> DONE
    *[So that others can receive help when they need it.]
         That’s right! Together, we can be Caring Commuters!
            -> DONE
            
==loadMCQ==
->DONE