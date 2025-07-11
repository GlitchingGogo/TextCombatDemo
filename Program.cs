using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

class Program
{
    static void Main()
    {
        // Declare Variables
        int playerHealth = 20;
        int enemyHealth = 20;
        int playerAttack = 0;
        int playerBaseAttack = 0;
        int playerBonusDamage = 0;
        int cyberwearDamage = 0;
        bool hackExpended = false;
        bool playerEscaped = false;
        bool playerEscapeFailed = false;
        bool lastStandUsed = false;
        bool playerStunned = false;
        bool enemyStunned = false;
        Random rnd = new Random();

        Console.WriteLine("Combat Start");
        Console.Write(Environment.NewLine);
        Console.Write(Environment.NewLine);
        Console.Write(Environment.NewLine);

        while (playerHealth > 0 && enemyHealth > 0)
        {
            // Player Main Actions
            Console.Write(Environment.NewLine);
            Console.WriteLine("-- Player Turn --");

            // Player stun check, if true pass turn
            if (playerStunned == true)
            {
                Console.WriteLine("You are Stunned!");
                playerStunned = false;
                Console.WriteLine("Player Health: " + playerHealth);
                Console.WriteLine("Enemy Health: " + enemyHealth);
                Console.Write(Environment.NewLine);
            }

            else
            {
                // Reset failed escape
                playerEscapeFailed = false;
                
                Console.WriteLine("Select a Main Action.");
                Console.WriteLine("====================");
                if (playerBonusDamage > 0)
                {
                    Console.WriteLine("(W)eapon - Deal 1d6 Damage. Add " + playerBonusDamage + " bonus damage from Hack.");
                }

                else
                {
                    Console.WriteLine("(W)eapon - Deal 1d6 Damage.");
                }

                if (hackExpended == true)
                {
                    Console.WriteLine("(H)ack - On Cooldown. Use your (W)eapon to recharge!");
                }

                else
                {
                    Console.WriteLine("(H)ack - Charge weapon for 1d4 bonus damage on a future turn. Cannot be stacked.");
                }

                if (playerHealth <= 10)
                {
                    Console.WriteLine("(C)yberwear - Needs Minimum 10 Health to Function.");
                }

                else
                {
                    Console.WriteLine("(C)yberwear - Burn Up to 10 Health, And Stun the Enemy on Their Next Turn");
                }

                Console.WriteLine("(E)scape - Take a Chance to Escape Combat. On a Failed Roll, Your Turn is Skipped.");
                Console.Write(Environment.NewLine);
                Console.WriteLine("Input anything else to skip this action.");
                Console.WriteLine("====================");

                string playerMainAction = Console.ReadLine();

                // Weapon -> 1d6 Damage to Enemy
                if (playerMainAction.ToLower() == "w" || playerMainAction.ToLower() == "weapon")
                {
                    playerAttack = rnd.Next(1, 6);

                    if (playerBonusDamage == 0)
                    {
                        Console.Write(Environment.NewLine);
                        enemyHealth -= playerAttack;
                        Console.WriteLine("You shoot the enemy for " + playerAttack + " damage.");
                        Console.WriteLine("Player Health: " + playerHealth);
                        Console.WriteLine("Enemy Health: " + enemyHealth);
                    }

                    // If hack buffed used add bonus damage and reset cooldown
                    if (playerBonusDamage > 0)
                    {
                        Console.Write(Environment.NewLine);
                        playerBaseAttack = playerAttack;
                        playerAttack = playerAttack += playerBonusDamage;
                        enemyHealth -= playerAttack;
                        Console.WriteLine("You shoot the enemy for " + playerAttack + " damage. (" + playerBaseAttack + " + " + playerBonusDamage + " bonus damage).");
                        Console.WriteLine("Hack Recharged!");
                        playerBonusDamage = 0;
                        hackExpended = false;
                        Console.WriteLine("Player Health: " + playerHealth);
                        Console.WriteLine("Enemy Health: " + enemyHealth);
                    }
                }

                // Hack -> 1d4 Bonus Damage on Next Turn (Buff)
                if (playerMainAction.ToLower() == "h" || playerMainAction.ToLower() == "hack")
                {
                    // Charge Weapon if Not Expended
                    if (hackExpended == true)
                    {
                        Console.Write(Environment.NewLine);
                        Console.WriteLine("You can't stack hacks! Use your bonus damage first.");
                        Console.WriteLine("Player Health: " + playerHealth);
                        Console.WriteLine("Enemy Health: " + enemyHealth);
                    }

                    if (hackExpended == false)
                    {
                        Console.Write(Environment.NewLine);
                        playerBonusDamage = rnd.Next(1, 4);
                        hackExpended = true;
                        Console.WriteLine("Weapon Charged with " + playerBonusDamage + " bonus damage");
                        Console.WriteLine("Player Health: " + playerHealth);
                        Console.WriteLine("Enemy Health: " + enemyHealth);
                    }
                }

                // Cyberwear -> Burn up to 10 Health, Stun Enemy (Skip Their Next Turn). If health is too low, cannot use
                if (playerMainAction.ToLower() == "c" || playerMainAction.ToLower() == "cyberwear")
                {
                    if (playerHealth <= 10)
                    {
                        Console.Write(Environment.NewLine);
                        Console.WriteLine("Cannot use Cyberwear! Health too low.");
                        Console.WriteLine("Player Health: " + playerHealth);
                        Console.WriteLine("Enemy Health: " + enemyHealth);
                    }

                    else
                    {
                        Console.Write(Environment.NewLine);
                        cyberwearDamage = rnd.Next(1, 10);
                        playerHealth -= cyberwearDamage;
                        enemyStunned = true;
                        Console.WriteLine("You stun the enemy, but take " + cyberwearDamage +" damage in the process.");
                        Console.WriteLine("Player Health: " + playerHealth);
                        Console.WriteLine("Enemy Health: " + enemyHealth);
                    }
                }

                // Escape -> Random Roll, On 4/5/6 (Success), Break Loop & End Game
                if (playerMainAction.ToLower() == "e" || playerMainAction.ToLower() == "escape")
                {
                    int playerEscapeRoll = rnd.Next(1, 6);
                    if (playerEscapeRoll >= 4)
                    {
                        playerEscaped = true;
                        break;
                    }
                    else
                    {
                        Console.Write(Environment.NewLine);
                        playerEscapeFailed = true;
                        Console.WriteLine("You failed to escape!" + "(You rolled a " + playerEscapeRoll + ")");
                        Console.WriteLine("Turn Skipped!");
                        Console.WriteLine("Player Health: " + playerHealth);
                        Console.WriteLine("Enemy Health: " + enemyHealth);
                    }
                }

                // Check if enemy is dead before allowing side action (End game on true)
                if (enemyHealth <= 0)
                {
                    break;
                }
                if (playerEscapeFailed == true)
                {

                }

                else
                {
                    // Player Side Actions
                    Console.Write(Environment.NewLine);
                    Console.WriteLine("====================");
                    Console.WriteLine("Select a Side Action.");
                    Console.WriteLine("(I)tem - Heal for 1d6 damage. Cannot heal past your maximum health (20).");
                    if (lastStandUsed == true)
                    {
                        Console.WriteLine("(A)bility - Charge Expended.");
                    }
                    else
                    {
                        Console.WriteLine("(A)bility - Last Stand: Drop to 1 Health, and deal 3d6 Damage. Can only be used once.");
                    }
                    Console.Write(Environment.NewLine);
                    Console.WriteLine("Input anything else to skip this action.");
                    Console.WriteLine("====================");
                    string playerSideAction = Console.ReadLine();

                    // Item Action -> Heal 1d6 if not at Max Health (20)
                    if (playerSideAction.ToLower() == "i" || playerSideAction.ToLower() == "item")
                    {
                        int playerHealAmount = rnd.Next(1, 6);
                        if (playerHealth == 20)
                        {
                            Console.Write(Environment.NewLine);
                            Console.WriteLine("Can't Overheal!");
                            Console.WriteLine("Player Health: " + playerHealth);
                            Console.WriteLine("Enemy Health: " + enemyHealth);
                            Console.Write(Environment.NewLine);
                        }

                        else
                        {
                            Console.Write(Environment.NewLine);
                            playerHealth += playerHealAmount;
                            if (playerHealth > 20)
                            {
                                playerHealth = 20;
                            }
                            Console.WriteLine("You heal " + playerHealAmount + " Health.");
                            Console.WriteLine("Player Health: " + playerHealth);
                            Console.WriteLine("Enemy Health: " + enemyHealth);
                            Console.Write(Environment.NewLine);
                        }
                    }

                    // Ability Side Action -> Last Stand, Drop to 1HP and attack for 3d6 damage. Cannot use if at 1 HP, 1 use.
                    if (playerSideAction.ToLower() == "a" || playerSideAction.ToLower() == "ability")
                    {
                        if (playerHealth == 1 && lastStandUsed == false)
                        {
                            Console.Write(Environment.NewLine);
                            Console.WriteLine("Can't use Last Stand! Health too low.");
                            Console.WriteLine("Player Health: " + playerHealth);
                            Console.WriteLine("Enemy Health: " + enemyHealth);
                            Console.Write(Environment.NewLine);
                        }

                        if (lastStandUsed == true)
                        {
                            Console.Write(Environment.NewLine);
                            Console.WriteLine("Already used Last Stand!");
                            Console.WriteLine("Player Health: " + playerHealth);
                            Console.WriteLine("Enemy Health: " + enemyHealth);
                        }

                        if (playerHealth > 1 && lastStandUsed == false)
                        {
                            Console.Write(Environment.NewLine);
                            playerHealth = 1;
                            playerAttack = rnd.Next(1, 18);
                            enemyHealth = enemyHealth -= playerAttack;
                            Console.WriteLine("Last Stand! You are crippled, but deal " + playerAttack + " damage to the enemy.");
                            Console.WriteLine("Player Health: " + playerHealth);
                            Console.WriteLine("Enemy Health: " + enemyHealth);
                            Console.Write(Environment.NewLine);
                        }
                    }
                }
            }

            // Enemy Turn Stuff
            if (enemyHealth > 0)
            {
                Console.WriteLine("-- Enemy Turn --");

                // Enemy Stun Check
                if (enemyStunned == true)
                {
                    Console.WriteLine("The enemy is stunned!");
                    enemyStunned = false;
                    Console.WriteLine("Player Health: " + playerHealth);
                    Console.WriteLine("Enemy Health: " + enemyHealth);
                }

                else
                {
                    // Decide enemy move
                    int enemyIntent = 1; // rnd.Next(1, 3);

                    // Enemy Attack (1d6)
                    if (enemyIntent == 1)
                    {
                        int enemyAttack = rnd.Next(1, 6);
                        playerHealth -= enemyAttack;
                        Console.WriteLine("The enemy attacks for " + enemyAttack + " damage.");
                        Console.WriteLine("Player Health: " + playerHealth);
                        Console.WriteLine("Enemy Health: " + enemyHealth);
                    }

                    // Enemy Heal (1d4) if not at Max Health (20)
                    if (enemyIntent == 2)
                    {
                        int enemyHealAmount = rnd.Next(1, 4);
                        if (enemyHealth == 20)
                        {
                            Console.Write(Environment.NewLine);
                            Console.WriteLine("Enemy Can't Overheal!");
                            Console.WriteLine("Player Health: " + playerHealth);
                            Console.WriteLine("Enemy Health: " + enemyHealth);
                        }

                        else
                        {
                            enemyHealth += enemyHealAmount;
                            Console.Write(Environment.NewLine);
                            Console.WriteLine("The enemy heals " + enemyHealAmount + " Health.");
                            Console.WriteLine("Player Health: " + playerHealth);
                            Console.WriteLine("Enemy Health: " + enemyHealth);
                        }
                    }

                    // Enemy Stun (Skip Player Turn)
                    if (enemyIntent == 3)
                    {
                        playerStunned = true;
                        Console.Write(Environment.NewLine);
                        Console.WriteLine("The enemy stuns you!");
                        Console.WriteLine("Player Health: " + playerHealth);
                        Console.WriteLine("Enemy Health: " + enemyHealth);
                    }
                }
            }
        }

        if (playerHealth > 0 && playerEscaped == false)
        {
            Console.Write(Environment.NewLine);
            Console.WriteLine("You Win!");
        }

        if (playerHealth > 0 && playerEscaped == true)
        {
            Console.Write(Environment.NewLine);
            Console.WriteLine("You Escaped.");
        }

        if (playerHealth <= 0)
        {
            Console.Write(Environment.NewLine);
            Console.WriteLine("You Died.");
        }
    }
}