->Bobby

VAR correctAnswer = ""

==Bobby==
~correctAnswer = 0
Yes?
    *[Do you need my help?]
        (smirks) 
            -> DONE
    *[Those glasses are lit.] 
        These are not for show…
            -> DONE
    *[(Grab their cane)] 
       Hey! What are you doing, let go. (He storms away)
            -> DONE
    *[I can help you.] 
        I’m good, thank you!  
            -> DONE
 
 ==Brock==
~correctAnswer = 0
Yes?
    *[Do you need any help?]
        That’ll be great! 
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
~correctAnswer = 0
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
        Yes please. (You guided him to the station staff)
            -> DONE

==NicholasR==
~correctAnswer = 0
English or Chinese?
    *[English]
        (smirks) 
            -> DONE
    *[Chinese] 
        Sorry I can’t speak that language.
            -> DONE
    *[None] 
        Bye then.
            -> DONE
 
 