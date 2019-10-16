CSC  		= csc
PRJC    = ResourceGenerator

SRC     = $(subst /,\,$(shell gfind src -name \*.cs))

# DEFINE
_DEFINE =
DEFINE  = $(addprefix /define:,$(_DEFINE))


default:
	$(CSC) -out:bin\$(PRJC).debug.exe -target:exe \
		-resource:$(PRJC).resources \
		-win32icon:resources\icon.ico \
		-define:DEBUG \
		$(DEFINE) \
		$(SRC)


release:
	$(CSC) -out:bin\$(PRJC).exe -target:winexe \
		-resource:$(PRJC).resources \
		-win32icon:resources\icon.ico \
		$(DEFINE) \
		$(SRC)


run:
	bin\$(PRJC).debug.exe
