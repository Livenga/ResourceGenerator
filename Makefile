CSC  		= csc
PRJC    = ResourceGenerator
_DEFINE = DEBUG

DEFINE  = $(addprefix /define:,$(_DEFINE))
SRC     = $(subst /,\,$(shell gfind src -name \*.cs))


all:
	$(CSC) /out:bin\$(PRJC).exe /target:exe $(DEFINE) $(SRC)

release:
	$(CSC) /out:bin\$(PRJC).win.exe /target:winexe $(SRC)

run:
	bin\$(PRJC).exe
