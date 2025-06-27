--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.4

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: contractors; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.contractors (
    id integer NOT NULL,
    symbol character varying(50) NOT NULL,
    name character varying(255) NOT NULL
);


ALTER TABLE public.contractors OWNER TO postgres;

--
-- Name: contractors_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.contractors_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.contractors_id_seq OWNER TO postgres;

--
-- Name: contractors_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.contractors_id_seq OWNED BY public.contractors.id;


--
-- Name: document_items; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.document_items (
    id integer NOT NULL,
    productname character varying(255) NOT NULL,
    unit character varying(50) NOT NULL,
    quantity numeric(12,2) NOT NULL,
    documentid integer NOT NULL
);


ALTER TABLE public.document_items OWNER TO postgres;

--
-- Name: documentitems_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.documentitems_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.documentitems_id_seq OWNER TO postgres;

--
-- Name: documentitems_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.documentitems_id_seq OWNED BY public.document_items.id;


--
-- Name: documents; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.documents (
    id integer NOT NULL,
    date date NOT NULL,
    symbol character varying(100) NOT NULL,
    contractorid integer NOT NULL
);


ALTER TABLE public.documents OWNER TO postgres;

--
-- Name: documents_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.documents_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.documents_id_seq OWNER TO postgres;

--
-- Name: documents_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.documents_id_seq OWNED BY public.documents.id;


--
-- Name: contractors id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.contractors ALTER COLUMN id SET DEFAULT nextval('public.contractors_id_seq'::regclass);


--
-- Name: document_items id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document_items ALTER COLUMN id SET DEFAULT nextval('public.documentitems_id_seq'::regclass);


--
-- Name: documents id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.documents ALTER COLUMN id SET DEFAULT nextval('public.documents_id_seq'::regclass);


--
-- Data for Name: contractors; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.contractors (id, symbol, name) FROM stdin;
28	MH	MacroHard
29	xD	xDD
30	Hello	World
7	D-POST	Doc Contractor2
6	ABC	Test Contractor1
14	I-POST	Item Contractor
2	KON002	Hurtownia Beta SAS
40	QWERTY	QWERTY
5	D-PUT	ZUS
41	WAS	WAS
42	AAAA	AAAA
43	firma	firma
44	nintendo	nintendo
45	sad	sda
46	ty	ja
47	ttqwre	qwreqwr
48	testowy test	testowy test
54	Tencent	Tencent
55	Orlen	Orlen
57	Konami	Konami
49	LOL	LOL
50	qwer	qwer
56	Orlen2	Orlen2
4	XYZ	BigFarm
52	zss	zss
58	PZPN	PZPN
59	PEGAZ	PEGAZ
60	WPR	WPR
61	WZG	WZG
62	test1213	tsast
63	zw	zw
64	twz	twz
65	TZO	TZO
67	WB	WARNER
1	KON001	Firma Alpha Sp. z o.o.
68	XTB +	XTB
11	YZ	ToUpdate
8	POSTED	Item Contractor
70	Sushi	Sushi
9	Intel	Intel
71	AMD	AMD
72	QWE	QWE
51	XTB	XTB
66	SFD	SFD
53	PegaZUS	PegaZ
74	PGE	PGE
69	ZERO123	ZERO
75	BMW	BMW
3	I-PUT	Putting Company12
76	OPEL	OPEL
73	RS	Rossman
\.


--
-- Data for Name: document_items; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.document_items (id, productname, unit, quantity, documentid) FROM stdin;
1	Monitor 25"	szt	13.00	1
6	Created Item	kg	10.00	5
2	Klawiatura	szt	1125.00	1
14	myszka	szt	1100.00	1
20	karta graficzna	szt	1.00	1
23	telefon	szt	321.00	1
24	telefon	szt	321.00	1
25	Test	test	21.00	1
27	długopis	szt	312.00	1
28	myszka	szt	12.00	1
29	Razer myszka	szt	12.00	1
30	ogórki	szt	12.00	1
26	TestA	test	21.00	1
38	X5	szt	51.00	35
39	x6	szt	12.00	35
32	L9	szt	1.00	28
31	Test	test	2.00	21
33	myszka	szt	2.00	21
34	Ryba	szt	123.00	32
35	Woda	l	150.00	32
3	Dysk SSD 1TB	szt	115.00	2
4	Mysz optyczna	szt	40.00	2
22	dysk HDD	szt	50.00	2
18	sfasfasf	szt	24.00	16
16	fasf	xd	2.00	18
36	Szampon	szt	1100.00	33
37	balsam	szt	510.00	33
17	kanapka	szt	2.00	23
21	taśma	szt	123.00	22
\.


--
-- Data for Name: documents; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.documents (id, date, symbol, contractorid) FROM stdin;
5	2025-06-13	DOC-ITEM	14
15	2025-06-16	test	6
18	2025-06-17	tagasgf	40
19	2025-06-18	dsad	44
20	2025-06-18	sfasf	49
23	2025-06-19	testowytest	1
24	2025-06-18	Mc	54
22	2025-06-19	testadasd	4
25	2025-06-19	Orlen1	55
26	2025-06-18	Orlen2	56
27	2025-06-22	dasf	30
16	2025-06-17	Techland	4
28	2025-06-23	PZPN	58
29	2025-06-23	DC-1	67
21	2025-06-18	X/2025	66
30	2025-06-24	XTB	68
31	2025-06-24	ZERO	3
32	2025-06-24	SushiKusi	70
2	2024-06-02	PZ/2/2024	72
33	2025-06-24	Rossman	73
34	2025-06-24	test1	28
1	2025-01-01	PZ/1/2025	74
35	2025-06-24	OPEL	76
17	2025-06-18	Alpha	73
\.


--
-- Name: contractors_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.contractors_id_seq', 76, true);


--
-- Name: documentitems_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.documentitems_id_seq', 39, true);


--
-- Name: documents_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.documents_id_seq', 35, true);


--
-- Name: contractors contractors_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.contractors
    ADD CONSTRAINT contractors_pkey PRIMARY KEY (id);


--
-- Name: document_items documentitems_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document_items
    ADD CONSTRAINT documentitems_pkey PRIMARY KEY (id);


--
-- Name: documents documents_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.documents
    ADD CONSTRAINT documents_pkey PRIMARY KEY (id);


--
-- Name: contractors uq_contractors_symbol; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.contractors
    ADD CONSTRAINT uq_contractors_symbol UNIQUE (symbol);


--
-- Name: documents fk_contractor; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.documents
    ADD CONSTRAINT fk_contractor FOREIGN KEY (contractorid) REFERENCES public.contractors(id) ON DELETE CASCADE;


--
-- Name: document_items fk_document; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document_items
    ADD CONSTRAINT fk_document FOREIGN KEY (documentid) REFERENCES public.documents(id) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

