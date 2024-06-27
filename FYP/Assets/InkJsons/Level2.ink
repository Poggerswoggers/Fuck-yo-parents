->Jolin

VAR correctAnswer = ""
VAR questionName = ""
EXTERNAL LoadQuestion(name)


==Grace==
~correctAnswer = 0
I saw someone putting their bags on the MRT seats.
    *[Ignore them !]
        Doesn’t feel right to me though. We should free up the seat for those who need it!
            -> DONE
    *[Ask them politely to put their bags on the floor instead.] 
        Absolutely! We should free up the seat for those who need it! 
            -> DONE
    *[You should put yours on the other seat too.] 
        The correct thing to do is to free up the seat for those who need it!
            -> DONE
    *[You should scold them to put their bags on the floor instead.] 
        Are you sure? That doesn’t seem right. We should be nice and ask them free up the seat for those who need it!
            -> DONE
            
==Jolin==
~correctAnswer = 0
Urm, hi there! Do you need help?
    *[Can you dance?]
        No, I just like wearing streetwear. 
            -> DONE
    *[How can I be a caring commuter?] 
        Always be alert , you’ll never know who might need your help.
            -> DONE
    *[How do you avoid being late when commuting?] 
        I always plan ahead for my travel time, so I leave my house early to avoid being late.
            -> DONE
    *[Have you seen someone who could use a helping hand?] 
        I saw someone walking around with a HEARING AID, he could use some help.
            -> DONE
              
==Davian==
~correctAnswer = 0
Urm, hi there! Do you need help?
    *[You look like you can play Basketball?]
        Thanks for the compliment.
            -> DONE
    *[Nothing.] 
        Orh ok lor.
            -> DONE
    *[Where are you headed?] 
        Going to play basketball with some friends. being late.
            -> DONE
    *[Who is your idol?] 
        The legend in my heart - Lebron James.
            -> DONE
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
==loadMCQ==
->DONE