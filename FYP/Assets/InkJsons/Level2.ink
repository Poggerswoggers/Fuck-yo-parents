VAR correctAnswer = ""

==YuYuan==
~correctAnswer = 1
…
    *[You seem nervous, do you need my help?]
        Yes ,thank you. 
        (You slowly guide her to a nearby station staff)
            -> DONE
    *[(Judge them by staring)] 
        (Stresses out)
            -> DONE
    *[(Shouts loudly) Hi! I can help you.] 
        (Stresses out) It’s ok…
            -> DONE
    *[Is something wrong?] 
        ...
            -> DONE
            
==Dave==
~correctAnswer = 2
(Sign language)
    *[(Thumbs up)]
        (Confused look) 
            -> DONE
    *[(Types on phone to ask if he needed help)] 
        (Smiles at you and nods, as you guided him to the exit)
            -> DONE
    *[(Exaggerate your movement)] 
        (Seems uneasy)
            -> DONE
    *[(Cover your face)] 
        (Scratches his head) 
            -> DONE
            
  ==Fatimah==
~correctAnswer = 4
(Seems shaken)
    *[(Stare at her and say nothing)]
        (Confused look) 
            -> DONE
    *[You look like a lost kid, do you need my help?] 
        I am not a kid! Please leave me alone!
            -> DONE
    *[You look lost, I can help.] 
        I am not lost ! I will find my way.
            -> DONE
    *[Hello, do you need my help?] 
        I am a little lost, thanks for helping. 
        (She gave you her CARA card as you contact her family members.)
            -> DONE
            
==Grace==
~correctAnswer = 2
What should you do when you see someone put their bags on the seats?
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
        That doesn’t seem right. We should be nice and ask them free up the seat for those who need it!
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
        I saw someone walking around with a <color=yellow>HEARING AID</color>, he could use some help.
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
            
==Jayden==
~correctAnswer = 0
Hello There!
    *[Have you seen anyone who could use some help?]
        Be patient and look closely, I think someone might be <color=yellow>NERVOUS</color>.
        ->DONE
    *[Give me your best joke!]
        I failed my spelling… so my teacher said: Simei (see me) after school!
        ->DONE
    *[Where are you rushing off to?]
        Back home to play games duh. I need to level up for the rewards!
        ->DONE
    *[Get out of the way man!]
        Wow that’s rude.
        ->DONE

==UncleTan==
~correctAnswer = 1
Why should we take public transport?
    *[So we can be more environmentally friendly]
        Nice one! Public Transport is a more environmentally friendly travel option than private cars and ride-hailing.
        ->DONE
    *[So we can sit eat and drink]
        I don’t think so though. You will be fined
        ->DONE
    *[So we can chat loudly with our friends]
        That isn’t very considerate of you. Remember that you are commuting with others!
        ->DONE
    *[So we could sleep when commuting]
        You should be alert to people who might need help! So you could give out seats to people who need it.
        ->DONE


==Isaac==
~correctAnswer = 0
Yes?
    *[What are you listening to?]
        New Jeans Hype Boy
        ->DONE
    *[Are you wearing a hearing aid?]
        Woah that’s rude…How could you make assumptions just like that?
        ->DONE
    *[Do you need any help?]
        Nope, but I did see someone walk past me with a <color=yellow>HEARING AID</color>.
        ->DONE
    *[I like your keychains]
        Thank you, I’m trying to collect more animals!
        ->DONE
            
==Jason==
~correctAnswer = 0
What’s good cuh?
    *[How do you make your commuting session more fun?]
        I play games on my phone, just make sure not to disturb the other commuters when you do it though.
        ->DONE
    *[Have you seen someone who could use a helping hand?]
        Take your time to observe , there could be people that are <color=yellow>NERVOUS</color> around you but it's not obvious.
        ->DONE
    *[What should I do when a train arrives?]
        You should stand to side and allow the commuters from the train to exit before entering.
        ->DONE
    *[Tell me a joke.]
        Nah fam, nobody tells me what to do, I’m a Sigma. 
        ->DONE          
    
==Liyue==
~correctAnswer = 0
Hey!
    *[Nothing.]
        Oh…
        ->DONE
    *[Where are you going?]
        I’m going to a music festival with my friends!
        ->DONE
    *[What can I do to become a better commuter?]
        Remember and practice the 3As actively!
        ->DONE
        
==Kyle==
~correctAnswer = 0
Skibidi.
    *[Go away.]
        No rizz.
        ->DONE
    *[What’s skibidi?]
        Nevermind.
        ->DONE
    *[What are the 3As?]
        Assess, Ask and Assist!
        ->DONE
        
==Jiayan==
~correctAnswer = 4
What should you NOT do when you see a commuter in need e.g, commuter with physical disability, pregnant etc?
    *[Stare at them]
        That’s one thing to NOT do, however all the behaviors listed are rude. 
        We should not be quick to judge or laugh at others, and should be caring to commuters in need instead.
        ->DONE
    *[Laugh at them]
        That’s one thing to NOT do, however all the behaviors listed are rude. 
        We should not be quick to judge or laugh at others, and should be caring to commuters in need instead.
        ->DONE
    *[Speak to them very loudly]
        That’s one thing to NOT do, however all the behaviors listed are rude. 
        We should not be quick to judge or laugh at others, and should be caring to commuters in need instead.
        ->DONE
    *[All of the above]
        Great job! All the behaviors listed are rude. 
        We should not be quick to judge or laugh at others, and should be caring to commuters in need instead. Together, we can be Caring Commuters!
        ->DONE
        
==OungOung== 
~correctAnswer = 0
I’m Batman.
    *[No you’re not]
        You’re no fun.
        ->DONE
    *[Oh? So where are you going now?]
        Gotham needs me.
        ->DONE
    *[Where is your batmobile?]
        I left it at home, since taking public transport is more environmentally friendly. 
        ->DONE
    *[Ok noted.]
        Justice never sleeps!
        ->DONE
        
==Edmund== 
~correctAnswer = 0
Hey! What’s up?
    *[Have you seen anyone who might need help?]
        I saw someone that looks <color=yellow>LOST</color> , maybe she might need some help?
        ->DONE
    *[Name one game off the top of your head!]
        Mario Odyssey!  
        ->DONE
    *[Where are you going?]
        I am currently heading to school to teach!
        ->DONE
        
==Fries== 
~correctAnswer = 0
Um…hi? Sorry I’m in a rush.
    *[Sorry to bother!]
      It's okay.
        ->DONE
        
==Terence== 
~correctAnswer = 0
What’s up?.
    *[The ceiling]
        …
        ->DONE
    *[Hey there! Where are you headed to?]
       I’m getting some chicken rice! 
        ->DONE
    *[Nothing.]
       Huh? 
        ->DONE
    *[Have you seen anyone who might need help?]
        I think someone LOOKS <color=yellow>CONFUSED AND LOST</color>. You might want to be patient with them too.
        ->DONE        
                 