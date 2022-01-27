﻿using LogicBuilder.Attributes;

namespace Contoso.Forms.Parameters
{
    public class CommandButtonParameters
    {
		public CommandButtonParameters
		(
			[Comments("ICommand property name.")]
			string command = "SubmitCommand",

			[Comments("Font Awesome icons - or some other icon set.")]
			[Domain("Ad,AddressBook,AddressCard,Adjust,AirFreshener,AlignCenter,AlignJustify,AlignLeft,AlignRight,Allergies,Ambulance,AmericanSignLanguageInterpreting,Anchor,AngleDoubleDown,AngleDoubleLeft,AngleDoubleRight,AngleDoubleUp,AngleDown,AngleLeft,AngleRight,AngleUp,Angry,Ankh,AppleAlt,Archive,Archway,ArrowAltCircleDown,ArrowAltCircleLeft,ArrowAltCircleRight,ArrowAltCircleUp,ArrowCircleDown,ArrowCircleLeft,ArrowCircleRight,ArrowCircleUp,ArrowDown,ArrowLeft,ArrowRight,ArrowUp,ArrowsAlt,ArrowsAltH,ArrowsAltV,AssistiveListeningSystems,Asterisk,At,Atlas,Atom,AudioDescription,Award,Baby,BabyCarriage,Backspace,Backward,Bacon,Bacteria,Bacterium,Bahai,BalanceScale,BalanceScaleLeft,BalanceScaleRight,Ban,BandAid,Barcode,Bars,BaseballBall,BasketballBall,Bath,BatteryEmpty,BatteryFull,BatteryHalf,BatteryQuarter,BatteryThreeQuarters,Bed,Beer,Bell,BellSlash,BezierCurve,Bible,Bicycle,Biking,Binoculars,Biohazard,BirthdayCake,Blender,BlenderPhone,Blind,Blog,Bold,Bolt,Bomb,Bone,Bong,Book,BookDead,BookMedical,BookOpen,BookReader,Bookmark,BorderAll,BorderNone,BorderStyle,BowlingBall,Box,BoxOpen,BoxTissue,Boxes,Braille,Brain,BreadSlice,Briefcase,BriefcaseMedical,BroadcastTower,Broom,Brush,Bug,Building,Bullhorn,Bullseye,Burn,Bus,BusAlt,BusinessTime,Calculator,Calendar,CalendarAlt,CalendarCheck,CalendarDay,CalendarMinus,CalendarPlus,CalendarTimes,CalendarWeek,Camera,CameraRetro,Campground,CandyCane,Cannabis,Capsules,Car,CarAlt,CarBattery,CarCrash,CarSide,Caravan,CaretDown,CaretLeft,CaretRight,CaretSquareDown,CaretSquareLeft,CaretSquareRight,CaretSquareUp,CaretUp,Carrot,CartArrowDown,CartPlus,CashRegister,Cat,Certificate,Chair,Chalkboard,ChalkboardTeacher,ChargingStation,ChartArea,ChartBar,ChartLine,ChartPie,Check,CheckCircle,CheckDouble,CheckSquare,Cheese,Chess,ChessBishop,ChessBoard,ChessKing,ChessKnight,ChessPawn,ChessQueen,ChessRook,ChevronCircleDown,ChevronCircleLeft,ChevronCircleRight,ChevronCircleUp,ChevronDown,ChevronLeft,ChevronRight,ChevronUp,Child,Church,Circle,CircleNotch,City,ClinicMedical,Clipboard,ClipboardCheck,ClipboardList,Clock,Clone,ClosedCaptioning,Cloud,CloudDownloadAlt,CloudMeatball,CloudMoon,CloudMoonRain,CloudRain,CloudShowersHeavy,CloudSun,CloudSunRain,CloudUploadAlt,Cocktail,Code,CodeBranch,Coffee,Cog,Cogs,Coins,Columns,Comment,CommentAlt,CommentDollar,CommentDots,CommentMedical,CommentSlash,Comments,CommentsDollar,CompactDisc,Compass,Compress,CompressAlt,CompressArrowsAlt,ConciergeBell,Cookie,CookieBite,Copy,Copyright,Couch,CreditCard,Crop,CropAlt,Cross,Crosshairs,Crow,Crown,Crutch,Cube,Cubes,Cut,Database,Deaf,Democrat,Desktop,Dharmachakra,Diagnoses,Dice,DiceD20,DiceD6,DiceFive,DiceFour,DiceOne,DiceSix,DiceThree,DiceTwo,DigitalTachograph,Directions,Disease,Divide,Dizzy,Dna,Dog,DollarSign,Dolly,DollyFlatbed,Donate,DoorClosed,DoorOpen,DotCircle,Dove,Download,DraftingCompass,Dragon,DrawPolygon,Drum,DrumSteelpan,DrumstickBite,Dumbbell,Dumpster,DumpsterFire,Dungeon,Edit,Egg,Eject,EllipsisH,EllipsisV,Envelope,EnvelopeOpen,EnvelopeOpenText,EnvelopeSquare,Equals,Eraser,Ethernet,EuroSign,ExchangeAlt,Exclamation,ExclamationCircle,ExclamationTriangle,Expand,ExpandAlt,ExpandArrowsAlt,ExternalLinkAlt,ExternalLinkSquareAlt,Eye,EyeDropper,EyeSlash,Fan,FastBackward,FastForward,Faucet,Fax,Feather,FeatherAlt,Female,FighterJet,File,FileAlt,FileArchive,FileAudio,FileCode,FileContract,FileCsv,FileDownload,FileExcel,FileExport,FileImage,FileImport,FileInvoice,FileInvoiceDollar,FileMedical,FileMedicalAlt,FilePdf,FilePowerpoint,FilePrescription,FileSignature,FileUpload,FileVideo,FileWord,Fill,FillDrip,Film,Filter,Fingerprint,Fire,FireAlt,FireExtinguisher,FirstAid,Fish,FistRaised,Flag,FlagCheckered,FlagUsa,Flask,Flushed,Folder,FolderMinus,FolderOpen,FolderPlus,Font,FootballBall,Forward,Frog,Frown,FrownOpen,FunnelDollar,Futbol,Gamepad,GasPump,Gavel,Gem,Genderless,Ghost,Gift,Gifts,GlassCheers,GlassMartini,GlassMartiniAlt,GlassWhiskey,Glasses,Globe,GlobeAfrica,GlobeAmericas,GlobeAsia,GlobeEurope,GolfBall,Gopuram,GraduationCap,GreaterThan,GreaterThanEqual,Grimace,Grin,GrinAlt,GrinBeam,GrinBeamSweat,GrinHearts,GrinSquint,GrinSquintTears,GrinStars,GrinTears,GrinTongue,GrinTongueSquint,GrinTongueWink,GrinWink,GripHorizontal,GripLines,GripLinesVertical,GripVertical,Guitar,HSquare,Hamburger,Hammer,Hamsa,HandHolding,HandHoldingHeart,HandHoldingMedical,HandHoldingUsd,HandHoldingWater,HandLizard,HandMiddleFinger,HandPaper,HandPeace,HandPointDown,HandPointLeft,HandPointRight,HandPointUp,HandPointer,HandRock,HandScissors,HandSparkles,HandSpock,Hands,HandsHelping,HandsWash,Handshake,HandshakeAltSlash,HandshakeSlash,Hanukiah,HardHat,Hashtag,HatCowboy,HatCowboySide,HatWizard,Hdd,HeadSideCough,HeadSideCoughSlash,HeadSideMask,HeadSideVirus,Heading,Headphones,HeadphonesAlt,Headset,Heart,HeartBroken,Heartbeat,Helicopter,Highlighter,Hiking,Hippo,History,HockeyPuck,HollyBerry,Home,Horse,HorseHead,Hospital,HospitalAlt,HospitalSymbol,HospitalUser,HotTub,Hotdog,Hotel,Hourglass,HourglassEnd,HourglassHalf,HourglassStart,HouseDamage,HouseUser,Hryvnia,ICursor,IceCream,Icicles,Icons,IdBadge,IdCard,IdCardAlt,Igloo,Image,Images,Inbox,Indent,Industry,Infinity,Info,InfoCircle,Italic,Jedi,Joint,JournalWhills,Kaaba,Key,Keyboard,Khanda,Kiss,KissBeam,KissWinkHeart,KiwiBird,Landmark,Language,Laptop,LaptopCode,LaptopHouse,LaptopMedical,Laugh,LaughBeam,LaughSquint,LaughWink,LayerGroup,Leaf,Lemon,LessThan,LessThanEqual,LevelDownAlt,LevelUpAlt,LifeRing,Lightbulb,Link,LiraSign,List,ListAlt,ListOl,ListUl,LocationArrow,Lock,LockOpen,LongArrowAltDown,LongArrowAltLeft,LongArrowAltRight,LongArrowAltUp,LowVision,LuggageCart,Lungs,LungsVirus,Magic,Magnet,MailBulk,Male,Map,MapMarked,MapMarkedAlt,MapMarker,MapMarkerAlt,MapPin,MapSigns,Marker,Mars,MarsDouble,MarsStroke,MarsStrokeH,MarsStrokeV,Mask,Medal,Medkit,Meh,MehBlank,MehRollingEyes,Memory,Menorah,Mercury,Meteor,Microchip,Microphone,MicrophoneAlt,MicrophoneAltSlash,MicrophoneSlash,Microscope,Minus,MinusCircle,MinusSquare,Mitten,Mobile,MobileAlt,MoneyBill,MoneyBillAlt,MoneyBillWave,MoneyBillWaveAlt,MoneyCheck,MoneyCheckAlt,Monument,Moon,MortarPestle,Mosque,Motorcycle,Mountain,Mouse,MousePointer,MugHot,Music,NetworkWired,Neuter,Newspaper,NotEqual,NotesMedical,ObjectGroup,ObjectUngroup,OilCan,Om,Otter,Outdent,Pager,PaintBrush,PaintRoller,Palette,Pallet,PaperPlane,Paperclip,ParachuteBox,Paragraph,Parking,Passport,Pastafarianism,Paste,Pause,PauseCircle,Paw,Peace,Pen,PenAlt,PenFancy,PenNib,PenSquare,PencilAlt,PencilRuler,PeopleArrows,PeopleCarry,PepperHot,Percent,Percentage,PersonBooth,Phone,PhoneAlt,PhoneSlash,PhoneSquare,PhoneSquareAlt,PhoneVolume,PhotoVideo,PiggyBank,Pills,PizzaSlice,PlaceOfWorship,Plane,PlaneArrival,PlaneDeparture,PlaneSlash,Play,PlayCircle,Plug,Plus,PlusCircle,PlusSquare,Podcast,Poll,PollH,Poo,PooStorm,Poop,Portrait,PoundSign,PowerOff,Pray,PrayingHands,Prescription,PrescriptionBottle,PrescriptionBottleAlt,Print,Procedures,ProjectDiagram,PumpMedical,PumpSoap,PuzzlePiece,Qrcode,Question,QuestionCircle,Quidditch,QuoteLeft,QuoteRight,Quran,Radiation,RadiationAlt,Rainbow,Random,Receipt,RecordVinyl,Recycle,Redo,RedoAlt,Registered,RemoveFormat,Reply,ReplyAll,Republican,Restroom,Retweet,Ribbon,Ring,Road,Robot,Rocket,Route,Rss,RssSquare,RubleSign,Ruler,RulerCombined,RulerHorizontal,RulerVertical,Running,RupeeSign,SadCry,SadTear,Satellite,SatelliteDish,Save,School,Screwdriver,Scroll,SdCard,Search,SearchDollar,SearchLocation,SearchMinus,SearchPlus,Seedling,Server,Shapes,Share,ShareAlt,ShareAltSquare,ShareSquare,ShekelSign,ShieldAlt,ShieldVirus,Ship,ShippingFast,ShoePrints,ShoppingBag,ShoppingBasket,ShoppingCart,Shower,ShuttleVan,Sign,SignInAlt,SignLanguage,SignOutAlt,Signal,Signature,SimCard,Sink,Sitemap,Skating,Skiing,SkiingNordic,Skull,SkullCrossbones,Slash,Sleigh,SlidersH,Smile,SmileBeam,SmileWink,Smog,Smoking,SmokingBan,Sms,Snowboarding,Snowflake,Snowman,Snowplow,Soap,Socks,SolarPanel,Sort,SortAlphaDown,SortAlphaDownAlt,SortAlphaUp,SortAlphaUpAlt,SortAmountDown,SortAmountDownAlt,SortAmountUp,SortAmountUpAlt,SortDown,SortNumericDown,SortNumericDownAlt,SortNumericUp,SortNumericUpAlt,SortUp,Spa,SpaceShuttle,SpellCheck,Spider,Spinner,Splotch,SprayCan,Square,SquareFull,SquareRootAlt,Stamp,Star,StarAndCrescent,StarHalf,StarHalfAlt,StarOfDavid,StarOfLife,StepBackward,StepForward,Stethoscope,StickyNote,Stop,StopCircle,Stopwatch,Stopwatch20,Store,StoreAlt,StoreAltSlash,StoreSlash,Stream,StreetView,Strikethrough,Stroopwafel,Subscript,Subway,Suitcase,SuitcaseRolling,Sun,Superscript,Surprise,Swatchbook,Swimmer,SwimmingPool,Synagogue,Sync,SyncAlt,Syringe,Table,TableTennis,Tablet,TabletAlt,Tablets,TachometerAlt,Tag,Tags,Tape,Tasks,Taxi,Teeth,TeethOpen,TemperatureHigh,TemperatureLow,Tenge,Terminal,TextHeight,TextWidth,Th,ThLarge,ThList,TheaterMasks,Thermometer,ThermometerEmpty,ThermometerFull,ThermometerHalf,ThermometerQuarter,ThermometerThreeQuarters,ThumbsDown,ThumbsUp,Thumbtack,TicketAlt,Times,TimesCircle,Tint,TintSlash,Tired,ToggleOff,ToggleOn,Toilet,ToiletPaper,ToiletPaperSlash,Toolbox,Tools,Tooth,Torah,ToriiGate,Tractor,Trademark,TrafficLight,Trailer,Train,Tram,Transgender,TransgenderAlt,Trash,TrashAlt,TrashRestore,TrashRestoreAlt,Tree,Trophy,Truck,TruckLoading,TruckMonster,TruckMoving,TruckPickup,Tshirt,Tty,Tv,Umbrella,UmbrellaBeach,Underline,Undo,UndoAlt,UniversalAccess,University,Unlink,Unlock,UnlockAlt,Upload,User,UserAlt,UserAltSlash,UserAstronaut,UserCheck,UserCircle,UserClock,UserCog,UserEdit,UserFriends,UserGraduate,UserInjured,UserLock,UserMd,UserMinus,UserNinja,UserNurse,UserPlus,UserSecret,UserShield,UserSlash,UserTag,UserTie,UserTimes,Users,UsersCog,UsersSlash,UtensilSpoon,Utensils,VectorSquare,Venus,VenusDouble,VenusMars,Vial,Vials,Video,VideoSlash,Vihara,Virus,VirusSlash,Viruses,Voicemail,VolleyballBall,VolumeDown,VolumeMute,VolumeOff,VolumeUp,VoteYea,VrCardboard,Walking,Wallet,Warehouse,Water,WaveSquare,Weight,WeightHanging,Wheelchair,Wifi,Wind,WindowClose,WindowMaximize,WindowMinimize,WindowRestore,WineBottle,WineGlass,WineGlassAlt,WonSign,Wrench,XRay,YenSign,YinYang")]
			[ParameterEditorControl(ParameterControlType.DomainAutoComplete)]
			string buttonIcon = "Save"
		)
		{
			Command = command;
			ButtonIcon = buttonIcon;
		}

		public string Command { get; set; }
		public string ButtonIcon { get; set; }
    }
}