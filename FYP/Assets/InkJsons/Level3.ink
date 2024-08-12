VAR correctAnswer = ""

==Bobby==
~correctAnswer = 1
Yes?
    *[Do you need my help?]
         That’ll be great! I appreciate it. 
         (You offered your shoulder as he held on as you guided him to the bus was finding)  
            -> DONE
    *[Those glasses are lit.] 
        These are not for show…
            -> DONE
    *[(Grab their cane)] 
       Hey! What are you doing?? 
       (He's irritated by your actions)
            -> DONE
    *[I can help you.] 
        I’m good, thank you!  
            -> DONE
 
 ==Brock==
~correctAnswer = 1
Yes?
    *[Do you need any help?]
        That’ll be great! 
        (you wait for him to explain and guide him to a nearby bus interchange staff)
            -> DONE
    *[Huh?] 
        (He stares at you blankly)
            -> DONE
    *[Let me assist you.] 
       No thank you.
            -> DONE
    *[You seem uneasy, I can help?] 
        Who said so?!
            -> DONE

 ==Raj ==
~correctAnswer = 4
Where am I headed again?
    *[No clue.]
        Hmm. (He seems frustrated)
            -> DONE
    *[Maybe you wanted some food?] 
        Did I now? 
            -> DONE
    *[I can lead you there!] 
      Uh I am not comfortable with that…
            -> DONE
    *[Can I help you in any way mister?] 
        Yes please. 
        (You guided him to the bus interchange staff)
            -> DONE
 ==Fred ==
~correctAnswer = 2
There’s so many people here…
    *[(Pretend not to notice)]
        (He ignores you)
            -> DONE
    *[Do you need any help?] 
       Yes, thank you. 
       (You slowly guide him to a commuter care room for him to relax)
            -> DONE
    *[Let me help!] 
      No… (He walks past you)
            -> DONE
    *[Are you looking for that person? (points at random stranger)] 
        Uh, no…
            -> DONE
 ==Ben ==
~correctAnswer = 3
Arg it hurts, I just want to go home.
    *[What hurts? Can I help?]
        It’s okay, I am just talking to myself…
            -> DONE
    *[Are you suffering? Can I help you?] 
      I appreciate your concern, but I don’t want to trouble you. 
      (he moves past you)
            -> DONE
    *[May I help you?] 
      That’ll make my day, thank you! 
      (you guide him to the bus he was looking for slowly as he held your shoulder for support)
            -> DONE
    *[Where do you live?] 
        I’m not telling you! Please move out of the way.
            -> DONE

==Nicholas==
~correctAnswer = 0
English or Chinese?
    *[English]
        (smirks) 
            -> DONE
    *[Chinese] 
        Sorry I can’t speak that language.
            -> DONE
    *[...] 
        Bye then.
            -> DONE

==Terry ==
~correctAnswer = 1
What should you do when you see young children (below 6 years old) on the train?
    *[Ask the caregiver/children if they need a seat]
        Nice one lah! Young children may not be able to balance well in a moving bus or train or might feel tired more easily
            -> DONE
    *[Ignore them] 
      You should be alert! Ask the caregiver or the children if they need a seat. 
            -> DONE
    *[Play with them] 
     You shouldn’t do that! They might fall and injure themselves! Instead ask the caregiver or the children if they need a seat. 
            -> DONE
    *[Stare at them] 
        I don’t think you should…that is rude. Instead ask the caregiver or the children if they need a seat. 
            -> DONE
            
==Darren==
~correctAnswer = 0
What's good my g.
    *[Why does everyone here look the same?]
       I’m not good at drawing anatomy. Just slap on a different hairstyle and I get away with it.
            -> DONE
    *[Have you seen anyone who looks like they need help?] 
       I did come across someone who looked <color=yellow>NERVOUS</color>.
            -> DONE
    *[Are you a capybara?] 
     No, but that is my spirit animal.
            -> DONE
    *[What are the four caring norms?] 
        Give time, Give care, Give a hand and Give Thanks!
            -> DONE
 
 ==Boon==
~correctAnswer = 0
Yo yo yo yo! 
    *[What should you do if you find a commuter that needs help?]
       Ask if they need your help, guide them to a station staff so that they can Assist the commuter!
            -> DONE
    *[Are you referencing something?] 
       Maybe? But if you are sneezing, someone must be thinking about you.
            -> DONE
    *[Have you seen anyone who looks like they need help?] 
    I saw someone wearing a <color=yellow>LANYARD (card around the neck)</color>.
            -> DONE
    *[What is your night routine?] 
        I look into the mirror and I see the ALPHA in me!
            -> DONE
 
 ==Claire==
~correctAnswer = 0
… (She quietly looks at you) 
    *[Sorry to disturb you!]
    ...
            -> DONE

 ==Peng==
~correctAnswer = 0
 Is the Heart Zone one of the places  at MRT stations and bus interchanges for commuters to give and receive help?
    *[Yes]
       That is correct.The Heart Zone helps to build awareness and is a place for commuters to give and receive help.
            -> DONE
    *[No] 
       That is incorrect.The Heart Zone helps to build awareness and is a place for commuters to give and receive help.
            -> DONE
 
 ==Sarah==
  ~correctAnswer = 0
    (She is currently texting, better not disturb her!)
     -> DONE