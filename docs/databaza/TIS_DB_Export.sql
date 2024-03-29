PGDMP  .                     {            fvl-radenie-vlakov    16.1    16.0 ?    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false                        0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false                       0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false                       1262    16506    fvl-radenie-vlakov    DATABASE     �   CREATE DATABASE "fvl-radenie-vlakov" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Slovak_Slovakia.1250';
 $   DROP DATABASE "fvl-radenie-vlakov";
                postgres    false            �            1259    16523    board_comments    TABLE     �   CREATE TABLE public.board_comments (
    id integer NOT NULL,
    user_id integer,
    text text NOT NULL,
    date timestamp with time zone,
    priority integer
);
 "   DROP TABLE public.board_comments;
       public         heap    postgres    false            �            1259    16522    board_comments_id_seq    SEQUENCE     �   CREATE SEQUENCE public.board_comments_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.board_comments_id_seq;
       public          postgres    false    218                       0    0    board_comments_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.board_comments_id_seq OWNED BY public.board_comments.id;
          public          postgres    false    217            �            1259    16539 	   templates    TABLE     �   CREATE TABLE public.templates (
    id integer NOT NULL,
    name character varying(30),
    destination character varying(30),
    max_leanght integer
);
    DROP TABLE public.templates;
       public         heap    postgres    false            �            1259    16538    templates_id_seq    SEQUENCE     �   CREATE SEQUENCE public.templates_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.templates_id_seq;
       public          postgres    false    222                       0    0    templates_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.templates_id_seq OWNED BY public.templates.id;
          public          postgres    false    221            �            1259    16554    train_comments    TABLE     s   CREATE TABLE public.train_comments (
    train_id integer NOT NULL,
    user_id integer,
    text text NOT NULL
);
 "   DROP TABLE public.train_comments;
       public         heap    postgres    false            �            1259    16548    trains    TABLE     	  CREATE TABLE public.trains (
    id integer NOT NULL,
    name character varying(30),
    destination character varying(30),
    state integer NOT NULL,
    date date,
    coll boolean NOT NULL,
    n_wagons integer,
    max_leanght integer,
    leanght integer
);
    DROP TABLE public.trains;
       public         heap    postgres    false            �            1259    16547    trains_id_seq    SEQUENCE     �   CREATE SEQUENCE public.trains_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE public.trains_id_seq;
       public          postgres    false    224                       0    0    trains_id_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE public.trains_id_seq OWNED BY public.trains.id;
          public          postgres    false    223            �            1259    16532    user_changes    TABLE     �   CREATE TABLE public.user_changes (
    id integer NOT NULL,
    user_id integer,
    change_type integer,
    used boolean NOT NULL,
    date timestamp with time zone NOT NULL
);
     DROP TABLE public.user_changes;
       public         heap    postgres    false            �            1259    16531    user_changes_id_seq    SEQUENCE     �   CREATE SEQUENCE public.user_changes_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public.user_changes_id_seq;
       public          postgres    false    220                       0    0    user_changes_id_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public.user_changes_id_seq OWNED BY public.user_changes.id;
          public          postgres    false    219            �            1259    16508    users    TABLE     �   CREATE TABLE public.users (
    id integer NOT NULL,
    name character varying(30) NOT NULL,
    mail text NOT NULL,
    privileges integer,
    pass text
);
    DROP TABLE public.users;
       public         heap    postgres    false            �            1259    16507    users_id_seq    SEQUENCE     �   CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.users_id_seq;
       public          postgres    false    216                       0    0    users_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
          public          postgres    false    215            �            1259    16568    wagon_comments    TABLE     s   CREATE TABLE public.wagon_comments (
    wagon_id integer NOT NULL,
    user_id integer,
    text text NOT NULL
);
 "   DROP TABLE public.wagon_comments;
       public         heap    postgres    false            �            1259    16562    wagons    TABLE        CREATE TABLE public.wagons (
    id integer NOT NULL,
    train_id integer,
    n_order integer,
    state integer NOT NULL
);
    DROP TABLE public.wagons;
       public         heap    postgres    false            �            1259    16561    wagons_id_seq    SEQUENCE     �   CREATE SEQUENCE public.wagons_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE public.wagons_id_seq;
       public          postgres    false    227                       0    0    wagons_id_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE public.wagons_id_seq OWNED BY public.wagons.id;
          public          postgres    false    226            <           2604    16526    board_comments id    DEFAULT     v   ALTER TABLE ONLY public.board_comments ALTER COLUMN id SET DEFAULT nextval('public.board_comments_id_seq'::regclass);
 @   ALTER TABLE public.board_comments ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    217    218    218            >           2604    16542    templates id    DEFAULT     l   ALTER TABLE ONLY public.templates ALTER COLUMN id SET DEFAULT nextval('public.templates_id_seq'::regclass);
 ;   ALTER TABLE public.templates ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    221    222    222            ?           2604    16551 	   trains id    DEFAULT     f   ALTER TABLE ONLY public.trains ALTER COLUMN id SET DEFAULT nextval('public.trains_id_seq'::regclass);
 8   ALTER TABLE public.trains ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    223    224    224            =           2604    16535    user_changes id    DEFAULT     r   ALTER TABLE ONLY public.user_changes ALTER COLUMN id SET DEFAULT nextval('public.user_changes_id_seq'::regclass);
 >   ALTER TABLE public.user_changes ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    219    220    220            ;           2604    16511    users id    DEFAULT     d   ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);
 7   ALTER TABLE public.users ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    215    216    216            @           2604    16565 	   wagons id    DEFAULT     f   ALTER TABLE ONLY public.wagons ALTER COLUMN id SET DEFAULT nextval('public.wagons_id_seq'::regclass);
 8   ALTER TABLE public.wagons ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    226    227    227            �          0    16523    board_comments 
   TABLE DATA           K   COPY public.board_comments (id, user_id, text, date, priority) FROM stdin;
    public          postgres    false    218   �G       �          0    16539 	   templates 
   TABLE DATA           G   COPY public.templates (id, name, destination, max_leanght) FROM stdin;
    public          postgres    false    222   �G       �          0    16554    train_comments 
   TABLE DATA           A   COPY public.train_comments (train_id, user_id, text) FROM stdin;
    public          postgres    false    225   �G       �          0    16548    trains 
   TABLE DATA           j   COPY public.trains (id, name, destination, state, date, coll, n_wagons, max_leanght, leanght) FROM stdin;
    public          postgres    false    224   H       �          0    16532    user_changes 
   TABLE DATA           L   COPY public.user_changes (id, user_id, change_type, used, date) FROM stdin;
    public          postgres    false    220   1H       �          0    16508    users 
   TABLE DATA           A   COPY public.users (id, name, mail, privileges, pass) FROM stdin;
    public          postgres    false    216   NH       �          0    16568    wagon_comments 
   TABLE DATA           A   COPY public.wagon_comments (wagon_id, user_id, text) FROM stdin;
    public          postgres    false    228    I       �          0    16562    wagons 
   TABLE DATA           >   COPY public.wagons (id, train_id, n_order, state) FROM stdin;
    public          postgres    false    227   I       	           0    0    board_comments_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.board_comments_id_seq', 1, false);
          public          postgres    false    217            
           0    0    templates_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.templates_id_seq', 1, false);
          public          postgres    false    221                       0    0    trains_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.trains_id_seq', 1, false);
          public          postgres    false    223                       0    0    user_changes_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.user_changes_id_seq', 1, false);
          public          postgres    false    219                       0    0    users_id_seq    SEQUENCE SET     :   SELECT pg_catalog.setval('public.users_id_seq', 3, true);
          public          postgres    false    215                       0    0    wagons_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.wagons_id_seq', 1, false);
          public          postgres    false    226            J           2606    16530 "   board_comments board_comments_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public.board_comments
    ADD CONSTRAINT board_comments_pkey PRIMARY KEY (id);
 L   ALTER TABLE ONLY public.board_comments DROP CONSTRAINT board_comments_pkey;
       public            postgres    false    218            N           2606    16546    templates templates_name_key 
   CONSTRAINT     W   ALTER TABLE ONLY public.templates
    ADD CONSTRAINT templates_name_key UNIQUE (name);
 F   ALTER TABLE ONLY public.templates DROP CONSTRAINT templates_name_key;
       public            postgres    false    222            P           2606    16544    templates templates_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.templates
    ADD CONSTRAINT templates_pkey PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.templates DROP CONSTRAINT templates_pkey;
       public            postgres    false    222            T           2606    16560 "   train_comments train_comments_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.train_comments
    ADD CONSTRAINT train_comments_pkey PRIMARY KEY (train_id);
 L   ALTER TABLE ONLY public.train_comments DROP CONSTRAINT train_comments_pkey;
       public            postgres    false    225            R           2606    16553    trains trains_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public.trains
    ADD CONSTRAINT trains_pkey PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.trains DROP CONSTRAINT trains_pkey;
       public            postgres    false    224            L           2606    16537    user_changes user_changes_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public.user_changes
    ADD CONSTRAINT user_changes_pkey PRIMARY KEY (id);
 H   ALTER TABLE ONLY public.user_changes DROP CONSTRAINT user_changes_pkey;
       public            postgres    false    220            B           2606    16519    users users_mail_key 
   CONSTRAINT     O   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_mail_key UNIQUE (mail);
 >   ALTER TABLE ONLY public.users DROP CONSTRAINT users_mail_key;
       public            postgres    false    216            D           2606    16517    users users_name_key 
   CONSTRAINT     O   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_name_key UNIQUE (name);
 >   ALTER TABLE ONLY public.users DROP CONSTRAINT users_name_key;
       public            postgres    false    216            F           2606    16521    users users_pass_key 
   CONSTRAINT     O   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pass_key UNIQUE (pass);
 >   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pass_key;
       public            postgres    false    216            H           2606    16515    users users_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pkey;
       public            postgres    false    216            X           2606    16574 "   wagon_comments wagon_comments_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.wagon_comments
    ADD CONSTRAINT wagon_comments_pkey PRIMARY KEY (wagon_id);
 L   ALTER TABLE ONLY public.wagon_comments DROP CONSTRAINT wagon_comments_pkey;
       public            postgres    false    228            V           2606    16567    wagons wagons_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public.wagons
    ADD CONSTRAINT wagons_pkey PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.wagons DROP CONSTRAINT wagons_pkey;
       public            postgres    false    227            Y           2606    16575 *   board_comments board_comments_user_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.board_comments
    ADD CONSTRAINT board_comments_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id) NOT VALID;
 T   ALTER TABLE ONLY public.board_comments DROP CONSTRAINT board_comments_user_id_fkey;
       public          postgres    false    4680    216    218            [           2606    16585 +   train_comments train_comments_train_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.train_comments
    ADD CONSTRAINT train_comments_train_id_fkey FOREIGN KEY (train_id) REFERENCES public.trains(id) NOT VALID;
 U   ALTER TABLE ONLY public.train_comments DROP CONSTRAINT train_comments_train_id_fkey;
       public          postgres    false    4690    225    224            \           2606    16590 *   train_comments train_comments_user_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.train_comments
    ADD CONSTRAINT train_comments_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id) NOT VALID;
 T   ALTER TABLE ONLY public.train_comments DROP CONSTRAINT train_comments_user_id_fkey;
       public          postgres    false    216    225    4680            Z           2606    16580 &   user_changes user_changes_user_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.user_changes
    ADD CONSTRAINT user_changes_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id) NOT VALID;
 P   ALTER TABLE ONLY public.user_changes DROP CONSTRAINT user_changes_user_id_fkey;
       public          postgres    false    220    4680    216            ^           2606    16605 *   wagon_comments wagon_comments_user_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.wagon_comments
    ADD CONSTRAINT wagon_comments_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id) NOT VALID;
 T   ALTER TABLE ONLY public.wagon_comments DROP CONSTRAINT wagon_comments_user_id_fkey;
       public          postgres    false    216    228    4680            _           2606    16600 +   wagon_comments wagon_comments_wagon_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.wagon_comments
    ADD CONSTRAINT wagon_comments_wagon_id_fkey FOREIGN KEY (wagon_id) REFERENCES public.wagons(id) NOT VALID;
 U   ALTER TABLE ONLY public.wagon_comments DROP CONSTRAINT wagon_comments_wagon_id_fkey;
       public          postgres    false    227    4694    228            ]           2606    16595    wagons wagons_train_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.wagons
    ADD CONSTRAINT wagons_train_id_fkey FOREIGN KEY (train_id) REFERENCES public.trains(id) NOT VALID;
 E   ALTER TABLE ONLY public.wagons DROP CONSTRAINT wagons_train_id_fkey;
       public          postgres    false    4690    227    224            �      x������ � �      �      x������ � �      �      x������ � �      �      x������ � �      �      x������ � �      �   �   x�-�ˍ! �33�`�}K!s�1lF�I���򓕶�� ���9/��>���y�L��o���o�$�F���������c@�i@M�s+��+ä��Y8��[�nJ$��s��u;-�����$�,B���}=��y��R�$CgQ*���2�Wȝ��O��%���a:�      �      x������ � �      �      x������ � �     